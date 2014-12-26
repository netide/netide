using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Project.Interop;
using NetIde.Services.Env;
using NetIde.Services.PackageManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;
using NetIde.Util.Forms;
using NetIde.Win32;
using ListView = System.Windows.Forms.ListView;

namespace NetIde.Services.Shell
{
    internal partial class NiShell : ServiceBase, INiShell, INiQuerySave
    {
        private const string AutomationAccessButtonName = "89db2dd3-10f4-43f7-a09c-8b1d1038f137";

        private readonly NiConnectionPoint<INiShellEvents> _connectionPoint = new NiConnectionPoint<INiShellEvents>();
        private readonly Dictionary<INiWindowPane, DockContent> _dockContents = new Dictionary<INiWindowPane, DockContent>();
        private readonly NiEnv _env;
        private Button _automationAccessButton;
        private bool _disposed;
        private NiPackageManager _packageManager;
        private int _preMessageFilterRecursion;
        private bool _pending;
        private readonly SynchronizationContext _synchronizationContext;

        public event EventHandler RequerySuggested;

        protected virtual void OnRequerySuggested(EventArgs e)
        {
            var ev = RequerySuggested;
            if (ev != null)
                ev(this, e);
        }

        public NiShell(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _env = (NiEnv)GetService(typeof(INiEnv));

            ((IServiceContainer)GetService(typeof(IServiceContainer))).AddService(
                typeof(INiQuerySave),
                this
            );

            Application.AddMessageFilter(new MessageFilter(this));

            MouseWheelMessageFilter.Install();

            _synchronizationContext = SynchronizationContext.Current;

            QueueRequery();

            _automationAccessButton = CreateAutomationAccessButton();
        }

        private Button CreateAutomationAccessButton()
        {
            // When we unit test from automation, no mouse or keyboard events
            // are triggered. The problem with this is that the status of commands
            // are never re-queried. We make this button available to the unit
            // test system so that the unit test can force an immediate requery.

            var result = new Button
            {
                Name = AutomationAccessButtonName,
                Text = AutomationAccessButtonName,
                Visible = false
            };

            ((NiEnv)GetService(typeof(INiEnv))).MainForm.Controls.Add(result);

            var handle = result.Handle;

            result.Click += (s, e) => OnRequerySuggested(EventArgs.Empty);

            return result;
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiShellEvents sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        public HResult CreateToolWindow(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation toolWindowOrientation, out INiWindowFrame toolWindow)
        {
            toolWindow = null;

            try
            {
                if (windowPane == null)
                    throw new ArgumentNullException("windowPane");

                var dockContent = new DockContent(windowPane, dockStyle, toolWindowOrientation)
                {
                    Site = new SiteProxy(this)
                };

                dockContent.Disposed += dockContent_Disposed;

                _dockContents.Add(windowPane, dockContent);

                toolWindow = dockContent.GetProxy();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        void dockContent_Disposed(object sender, EventArgs e)
        {
            _dockContents.Remove(((DockContent)sender).WindowPane);
        }

        public HResult BrowseForFolder(string title, NiBrowseForFolderOptions options, out string selectedFolder)
        {
            selectedFolder = null;

            try
            {
                var browser = new FolderBrowser
                {
                    Title = title,
                    ShowEditBox = (options & NiBrowseForFolderOptions.HideEditBox) == 0,
                    ShowNewFolderButton = (options & NiBrowseForFolderOptions.HideNewFolderButton) == 0
                };

                if (browser.ShowDialog(GetActiveWindow()) == DialogResult.OK)
                {
                    selectedFolder = browser.SelectedPath;

                    return HResult.OK;
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public IWin32Window GetActiveWindow()
        {
            return new NativeWindowWrapper(NativeMethods.GetActiveWindow());
        }

        public HResult SaveDocDataToFile(NiSaveMode mode, INiPersistFile persistFile, string fileName, out string newFileName, out bool saved)
        {
            newFileName = null;
            saved = false;

            try
            {
                bool isDirty;
                ErrorUtil.ThrowOnFailure(persistFile.IsDirty(out isDirty));

                if (!isDirty)
                {
                    newFileName = fileName;
                    saved = true;

                    return HResult.OK;
                }

                switch (mode)
                {
                    case NiSaveMode.SaveAs:
                    case NiSaveMode.SaveCopyAs:
                        throw new NotImplementedException();

                    case NiSaveMode.Save:
                        INiHierarchy hier;
                        INiWindowFrame windowFrame;
                        ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).IsDocumentOpen(
                            fileName,
                            out hier,
                            out windowFrame
                        ));

                        if (hier != null)
                        {
                            NiQuerySaveResult querySaveResult;
                            var hr = QuerySaveViaDialog(new[] { hier }, out querySaveResult);

                            if (ErrorUtil.Failure(hr))
                                return hr;

                            switch (querySaveResult)
                            {
                                case NiQuerySaveResult.Cancel:
                                    return HResult.OK;

                                case NiQuerySaveResult.DoNotSave:
                                    // We pretend the document was saved if the
                                    // user asked us not to save the document.

                                    saved = true;
                                    return HResult.OK;
                            }
                        }
                        break;
                }

                newFileName = fileName;
                saved = true;

                return persistFile.Save(newFileName, true);
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult QuerySaveViaDialog(INiHierarchy[] hiers, out NiQuerySaveResult result)
        {
            result = NiQuerySaveResult.Cancel;

            try
            {
                if (hiers == null)
                    throw new ArgumentNullException("hiers");

                switch (SaveHierarchiesForm.ShowDialog(this, hiers))
                {
                    case DialogResult.Yes: result = NiQuerySaveResult.Save; break;
                    case DialogResult.No: result = NiQuerySaveResult.DoNotSave; break;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetWindowFrameForWindowPane(INiWindowPane windowPane, out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                DockContent dockContent;
                if (_dockContents.TryGetValue(windowPane, out dockContent))
                    windowFrame = dockContent.GetProxy();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult BroadcastPreMessageFilter(ref NiMessage message)
        {
            try
            {
                if (_preMessageFilterRecursion > 0)
                    return HResult.False;

                _preMessageFilterRecursion++;

                try
                {
                    // We delay resolve the package manager because it is created
                    // after NiShell is.

                    if (_packageManager == null)
                        _packageManager = (NiPackageManager)GetService(typeof(INiPackageManager));

                    if (MessageFilterUtil.InvokeMessageFilter(ref message))
                        return HResult.OK;

                    foreach (var package in _packageManager.Packages)
                    {
                        var preMessageFilter = package.Package as INiPreMessageFilter;
                        if (
                            preMessageFilter != null &&
                            ErrorUtil.ThrowOnFailure(preMessageFilter.PreFilterMessage(ref message))
                        )
                            return HResult.OK;
                    }

                    return HResult.False;
                }
                finally
                {
                    _preMessageFilterRecursion--;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetDocumentWindowIterator(out INiIterator<INiWindowFrame> iterator)
        {
            return _env.MainForm.GetDocumentWindowIterator(out iterator);
        }

        public HResult InvalidateRequerySuggested()
        {
            try
            {
                QueueRequery();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void QueueRequery()
        {
            if (!_pending)
            {
                _pending = true;
                _synchronizationContext.Post(Requery, null);
            }
        }

        private void Requery(object state)
        {
            _pending = false;

            OnRequerySuggested(EventArgs.Empty);
            _connectionPoint.ForAll(p => p.OnRequerySuggested());
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_automationAccessButton != null)
                {
                    _automationAccessButton.Dispose();
                    _automationAccessButton = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        private class MessageFilter : IMessageFilter
        {
            private readonly NiShell _shell;
            private readonly HashSet<ListView> _installedListViews = new HashSet<ListView>();

            public MessageFilter(NiShell shell)
            {
                _shell = shell;
            }

            public bool PreFilterMessage(ref Message m)
            {
                PreFilterRequery(ref m);
                return PreFilterBroadcast(ref m);
            }

            private bool PreFilterBroadcast(ref Message m)
            {
                if (_shell._preMessageFilterRecursion > 0)
                    return false;

                _shell._preMessageFilterRecursion++;

                try
                {
                    NiMessage message = m;

                    bool processed = ErrorUtil.ThrowOnFailure(_shell.BroadcastPreMessageFilter(ref message));

                    m = message;

                    return processed;
                }
                finally
                {
                    _shell._preMessageFilterRecursion--;
                }
            }

            private void PreFilterRequery(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_KEYUP:
                    case WM_KILLFOCUS:
                    case WM_LBUTTONUP:
                    case WM_MBUTTONUP:
                    case WM_MENURBUTTONUP:
                    case WM_MOUSEHWHEEL:
                    case WM_MOUSEWHEEL:
                    case WM_NCLBUTTONUP:
                    case WM_NCMBUTTONUP:
                    case WM_NCRBUTTONUP:
                    case WM_NCXBUTTONUP:
                    case WM_RBUTTONUP:
                    case WM_SETFOCUS:
                    case WM_SYSKEYUP:
                    case WM_XBUTTONUP:
                        _shell.QueueRequery();
                        break;

                    default:
                        InstallListViewListener(ref m);
                        break;
                }
            }

            private void InstallListViewListener(ref Message m)
            {
                // This is a hack. To implement updating command states, NiShell
                // executes a requery when certain window messages arrive.
                // One of the messages on which this is triggered is the
                // WM_L/RBUTTONUP. The problem is that the ListView does not send
                // these; it only sends the WM_L/RBUTTONDOWN, and we cannot use
                // these because the state of the list view may be updated in
                // a manner that is necessary for the commands to be updated
                // correctly (e.g. a change of selection). To work around this,
                // we add a MouseUp event to every list view that we see.

                var listView = Control.FromHandle(m.HWnd) as ListView;
                if (listView == null || !_installedListViews.Add(listView))
                    return;

                listView.MouseUp += listView_MouseUp;
                listView.Disposed += listView_Disposed;
            }

            void listView_MouseUp(object sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                    _shell.QueueRequery();
            }

            void listView_Disposed(object sender, EventArgs e)
            {
                _installedListViews.Remove((ListView)sender);
            }

            private const int WM_KEYUP = 0x101;
            private const int WM_SYSKEYUP = 0x105;
            private const int WM_LBUTTONUP = 0x202;
            private const int WM_MBUTTONUP = 0x208;
            private const int WM_MENURBUTTONUP = 0x122;
            private const int WM_NCLBUTTONUP = 0xA2;
            private const int WM_NCMBUTTONUP = 0xA8;
            private const int WM_NCRBUTTONUP = 0xA5;
            private const int WM_RBUTTONUP = 0x205;
            private const int WM_XBUTTONUP = 0x20C;
            private const int WM_MOUSEWHEEL = 0x20A;
            private const int WM_MOUSEHWHEEL = 0x20E;
            private const int WM_SETFOCUS = 0x7;
            private const int WM_KILLFOCUS = 0x8;
            private const int WM_NCXBUTTONUP = 0xAC;
        }

        private class NativeWindowWrapper : IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public NativeWindowWrapper(IntPtr handle)
            {
                Handle = handle;
            }
        }
    }
}
