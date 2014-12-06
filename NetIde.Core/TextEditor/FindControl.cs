using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using GdiPresentation;
using NetIde.Core.Services.Finder;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;
using Size = System.Drawing.Size;

namespace NetIde.Core.TextEditor
{
    internal partial class FindControl : NetIde.Util.Forms.UserControl
    {
        public static void Show(IServiceProvider serviceProvider, Control parent, NiFindOptions options, NiFindOptions optionsMask, INiFindTarget findTarget)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (findTarget == null)
                throw new ArgumentNullException("findTarget");

            var control = parent.Controls.OfType<FindControl>().SingleOrDefault();

            if (control == null)
            {
                control = new FindControl
                {
                    Anchor = AnchorStyles.Right,
                    Site = new SiteProxy(serviceProvider)
                };

                control.Left = parent.ClientSize.Width - control.Width;

                parent.Controls.Add(control);
            }

            control._findManager.FindTarget = findTarget;
            control._findManager.SetOptions(options, optionsMask);

            control._findWhat.Focus();
        }

        private FindManager _findManager;
        private ComboBoxInput _findWhat;
        private ComboBoxInput _replaceWith;
        private GdiButton _toggleReplace;
        private GdiButton _findNext;
        private GdiButton _close;
        private GdiButton _replaceNext;
        private GdiButton _replaceAll;
        private GdiButton _matchCase;
        private GdiButton _matchWholeWord;
        private GdiButton _useRegularExpressions;
        private FindMode _findMode;
        private StackPanel _findRow;
        private StackPanel _replaceRow;
        private StackPanel _optionsRow;

        public FindControl()
        {
            InitializeComponent();

            _elementControl.Content = BuildContent();
            _elementControl.PreferredSizeChanged += _elementControl_PreferredSizeChanged;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    (_findMode == FindMode.Find ? _findNext : _replaceNext).PerformClick();
                    return true;

                case Keys.Escape:
                    _close.PerformClick();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        void _elementControl_PreferredSizeChanged(object sender, PreferredSizeChangedEventArgs e)
        {
            var size = new Size(
                _elementControl.Content.DesiredSize.Width + 1,
                _elementControl.Content.DesiredSize.Height + 1
            );

            SetBounds(
                Parent.ClientSize.Width - size.Width,
                0,
                size.Width,
                size.Height
            );
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _findManager = new FindManager(new View(this));
            }
        }

        void _toggleReplace_Click(object sender, EventArgs e)
        {
            if (_findMode == FindMode.Find)
            {
                _findManager.SetOptions(NiFindOptions.Replace, NiFindOptions.ActionMask);
                SetMode(FindMode.Replace);
            }
            else
            {
                _findManager.SetOptions(NiFindOptions.Find, NiFindOptions.ActionMask);
                SetMode(FindMode.Find);
            }
        }

        private void SetMode(FindMode findMode)
        {
            _findMode = findMode;
            
            _toggleReplace.Bitmap = _findMode == FindMode.Find ? ToggleDownImage : ToggleUpImage;

            _replaceRow.Visibility = _findMode == FindMode.Replace ? Visibility.Visible : Visibility.Collapsed;
            _replaceWith.Control.Visible = _findMode == FindMode.Replace;
        }

        void _findNext_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.FindNext);
        }

        void _close_Click(object sender, EventArgs e)
        {
            Parent.Controls.Remove(this);
            Dispose();
        }

        void _replaceNext_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.Replace);
        }

        void _replaceAll_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.ReplaceAll);
        }

        private void PerformFind(FindAction action)
        {
            _findManager.PerformFind(action);
        }

        void _matchCase_IsCheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.MatchCase, _matchCase.IsChecked);
        }

        void _matchWholeWord_IsCheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.WholeWord, _matchWholeWord.IsChecked);
        }

        void _useRegularExpressions_IsCheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.RegExp, _useRegularExpressions.IsChecked);
        }

        private void SetOption(NiFindOptions option, bool value)
        {
            if (value)
                _findManager.Options |= option;
            else
                _findManager.Options &= ~option;
        }

        private class View : IFindView
        {
            private readonly FindControl _control;
            private string[] _lookAtFileTypesHistory;
            private string[] _lookInHistory;

            public View(FindControl control)
            {
                _control = control;
            }

            public string GetFindWhatText()
            {
                return _control._findWhat.Control.Text;
            }

            public string GetLookAtFileTypesText()
            {
                return String.Empty;
            }

            public string GetLookInText()
            {
                return String.Empty;
            }

            public string GetReplaceWithText()
            {
                return _control._replaceWith.Control.Text;
            }

            public string[] GetFindWhatHistory()
            {
                return GetHistory(_control._findWhat.Control);
            }

            public string[] GetLookAtFileTypesHistory()
            {
                return _lookAtFileTypesHistory;
            }

            public string[] GetLookInHistory()
            {
                return _lookInHistory;
            }

            public string[] GetReplaceWithHistory()
            {
                return GetHistory(_control._replaceWith.Control);
            }

            private string[] GetHistory(ComboBox comboBox)
            {
                var history = comboBox.Items.Cast<string>().ToList();

                if (comboBox.Text.Length > 0)
                {
                    int index = history.IndexOf(comboBox.Text);

                    if (index != -1)
                        history.RemoveAt(index);

                    history.Insert(0, comboBox.Text);
                }

                if (history.Count > 20)
                    history.RemoveRange(20, history.Count - 20);

                return history.ToArray();
            }

            public void BeginUpdate()
            {
            }

            public void EndUpdate()
            {
            }

            public void LoadFindWhatHistory(string[] history)
            {
                LoadHistory(_control._findWhat.Control, history);
            }

            public void LoadLookAtFileTypesHistory(string[] history)
            {
                _lookAtFileTypesHistory = history;
            }

            public void LoadLookInHistory(string[] history)
            {
                _lookInHistory = history;
            }

            public void LoadReplaceWithHistory(string[] history)
            {
                LoadHistory(_control._replaceWith.Control, history);
            }

            private void LoadHistory(ComboBox comboBox, string[] history)
            {
                foreach (string line in history)
                {
                    comboBox.Items.Add(line);
                }

                if (comboBox.Items.Count > 0)
                    comboBox.SelectedIndex = 0;
            }

            public void NoMoreOccurrences()
            {
                ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).ShowMessageBox(
                    Labels.NoMoreOccurrences,
                    null,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                ));
            }

            public void SetIncludeSubFolders(bool value)
            {
            }

            public void SetKeepOpen(bool value)
            {
            }

            public void SetMatchCase(bool value)
            {
                _control._matchCase.IsChecked = value;
            }

            public void SetMatchWholeWord(bool value)
            {
                _control._matchWholeWord.IsChecked = value;
            }

            public void SetMode(FindMode findMode)
            {
                _control.SetMode(findMode);
            }

            public void SetTarget(FindTarget findTarget)
            {
            }

            public void SetUseRegularExpressions(bool value)
            {
                _control._useRegularExpressions.IsChecked = value;
            }

            public object GetService(Type serviceType)
            {
                return _control.Site.GetService(serviceType);
            }
        }
    }
}
