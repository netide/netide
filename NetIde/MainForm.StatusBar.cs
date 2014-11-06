using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde
{
    partial class MainForm
    {
        private class NiStatusBar : ServiceBase, INiStatusBar
        {
            private readonly MainForm _mainForm;
            private readonly Status _defaultStatus = new Status();
            private readonly Dictionary<INiStatusBarUser, Status> _statuses = new Dictionary<INiStatusBarUser, Status>();
            private INiStatusBarUser _currentUser;

            public NiStatusBar(MainForm mainForm, IServiceProvider serviceProvider)
                : base(serviceProvider)
            {
                _mainForm = mainForm;

                ApplyStatus(_defaultStatus);

                new WindowPaneSelectionListener(this, _mainForm._windowPaneSelection);
            }

            private void OnActiveDocumentChanged()
            {
                var currentPane = _mainForm._windowPaneSelection.ActiveDocument;
                _currentUser = currentPane as INiStatusBarUser;

                Status status;

                if (_currentUser == null)
                {
                    status = _defaultStatus;
                }
                else
                {
                    if (!_statuses.TryGetValue(_currentUser, out status))
                    {
                        status = new Status();
                        _statuses.Add(_currentUser, status);

                        INiWindowFrame windowFrame;
                        ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).GetWindowFrameForWindowPane(
                            currentPane,
                            out windowFrame
                        ));

                        Debug.Assert(windowFrame != null);

                        new WindowPaneListener(this, _currentUser, windowFrame);
                    }

                    ErrorUtil.ThrowOnFailure(_currentUser.SetInfo());
                }

                ApplyStatus(status);
            }

            private Status GetCurrentStatus()
            {
                if (_currentUser != null)
                    return _statuses[_currentUser];

                return _defaultStatus;
            }

            private void ApplyStatus(Status status)
            {
                _mainForm._statusStripText.Text = status.Text ?? Labels.StatusBarDefaultStatus;

                _mainForm._statusStripInsertMode.Visible = status.InsertMode != NiInsertMode.None;

                switch (status.InsertMode)
                {
                    case NiInsertMode.Insert: _mainForm._statusStripInsertMode.Text = Labels.InsertModeInsert; break;
                    case NiInsertMode.Overwrite: _mainForm._statusStripInsertMode.Text = Labels.InsertModeOverwrite; break;
                }

                _mainForm._statusStripLine.Visible = status.Line.HasValue;
                if (status.Line.HasValue)
                    _mainForm._statusStripLine.Text = String.Format(Labels.StatusBarLine, status.Line.Value);
                _mainForm._statusStripCharIndex.Visible = status.CharIndex.HasValue;
                if (status.CharIndex.HasValue)
                    _mainForm._statusStripCharIndex.Text = String.Format(Labels.StatusBarCharIndex, status.CharIndex.Value);
                _mainForm._statusStripIndex.Visible = status.Index.HasValue;
                if (status.Index.HasValue)
                    _mainForm._statusStripIndex.Text = String.Format(Labels.StatusBarIndex, status.Index.Value);
            }

            public HResult Clear()
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.Text = null;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetText(out string text)
            {
                text = null;

                try
                {
                    text = GetCurrentStatus().Text;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult IsCurrentUser(INiStatusBarUser user, out bool isCurrent)
            {
                isCurrent = false;

                try
                {
                    isCurrent = user == _currentUser;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult CreateProgress(out INiStatusBarProgress progress)
            {
                progress = null;

                try
                {
                    var label = new ToolStripStatusLabel
                    {
                        Visible = false
                    };
                    var progressBar = new ToolStripProgressBar
                    {
                        Visible = false
                    };

                    int index = _mainForm._statusStrip.Items.IndexOf(_mainForm._statusStripText);

                    _mainForm._statusStrip.Items.Insert(
                        index + 1,
                        label
                    );
                    _mainForm._statusStrip.Items.Insert(
                        index + 2,
                        progressBar
                    );

                    progress = new NiStatusBarProgress(label, progressBar);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetInsertMode(NiInsertMode mode)
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.InsertMode = mode;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult HideLineColChar()
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.Line = null;
                    status.Index = null;
                    status.CharIndex = null;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetLineChar(int line, int charIndex)
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.Line = line;
                    status.Index = null;
                    status.CharIndex = charIndex;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetLineColChar(int line, int index, int charIndex)
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.Line = line;
                    status.Index = index;
                    status.CharIndex = charIndex;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetText(string text)
            {
                try
                {
                    var status = GetCurrentStatus();

                    status.Text = String.IsNullOrEmpty(text) ? null : text;

                    ApplyStatus(status);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            private class NiStatusBarProgress : ServiceObject, INiStatusBarProgress
            {
                private ToolStripStatusLabel _label;
                private ToolStripProgressBar _progressBar;
                private bool _disposed;

                public NiStatusBarProgress(ToolStripStatusLabel label, ToolStripProgressBar progressBar)
                {
                    _label = label;
                    _progressBar = progressBar;
                }

                public HResult Update(string label, int progress, int total)
                {
                    try
                    {
                        _label.Visible = !String.IsNullOrEmpty(label);
                        _label.Text = label;

                        _progressBar.Visible = true;
                        _progressBar.Minimum = 0;
                        _progressBar.Maximum = total;
                        _progressBar.Value = progress;

                        return HResult.OK;
                    }
                    catch (Exception ex)
                    {
                        return ErrorUtil.GetHResult(ex);
                    }
                }

                protected override void Dispose(bool disposing)
                {
                    if (!_disposed)
                    {
                        if (_label != null)
                        {
                            _label.Owner.Items.Remove(_label);
                            _label = null;
                        }
                        if (_progressBar != null)
                        {
                            _progressBar.Owner.Items.Remove(_progressBar);
                            _progressBar = null;
                        }

                        _disposed = true;
                    }

                    base.Dispose(disposing);
                }
            }

            private class Status
            {
                public string Text { get; set; }
                public NiInsertMode InsertMode { get; set; }
                public int? Line { get; set; }
                public int? Index { get; set; }
                public int? CharIndex { get; set; }
            }

            private class WindowPaneSelectionListener : NiEventSink, INiWindowPaneSelectionNotify
            {
                private readonly NiStatusBar _statusBar;

                public WindowPaneSelectionListener(NiStatusBar statusBar, INiConnectionPoint connectionPoint)
                    : base(connectionPoint)
                {
                    _statusBar = statusBar;
                }

                public void OnActiveDocumentChanged()
                {
                    _statusBar.OnActiveDocumentChanged();
                }
            }

            private class WindowPaneListener : NiEventSink, INiWindowFrameNotify
            {
                private readonly NiStatusBar _statusBar;
                private readonly INiStatusBarUser _statusBarUser;

                public WindowPaneListener(NiStatusBar statusBar, INiStatusBarUser statusBarUser, INiConnectionPoint connectionPoint)
                    : base(connectionPoint)
                {
                    _statusBar = statusBar;
                    _statusBarUser = statusBarUser;
                }

                public void OnShow(NiWindowShow action)
                {
                    if (action == NiWindowShow.Close)
                    {
                        _statusBar._statuses.Remove(_statusBarUser);
                        _statusBar._currentUser = null;
                        _statusBar.ApplyStatus(_statusBar.GetCurrentStatus());
                    }
                }

                public void OnSize()
                {
                }

                public void OnClose(NiFrameCloseMode closeMode, ref bool cancel)
                {
                }
            }
        }
    }
}
