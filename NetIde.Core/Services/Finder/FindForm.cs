using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.Services.Finder
{
    internal partial class FindForm : DialogForm
    {
        private static readonly string[] DefaultLookInOptions = { Labels.CurrentDocument, Labels.AllOpenDocuments, Labels.EntireProject };

        private readonly Dictionary<Control, Padding> _margins = new Dictionary<Control, Padding>();
        private FindManager _findManager;

        public FindForm()
        {
            InitializeComponent();

            _lookIn.Items.AddRange(DefaultLookInOptions);
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

        public void SetOptions(NiFindOptions options, NiFindOptions optionsMask)
        {
            _findManager.SetOptions(options, optionsMask);
        }

        private void _modeFind_Click(object sender, EventArgs e)
        {
            SetOptions(NiFindOptions.Find, NiFindOptions.ActionMask);
        }

        private void _modeFindReplace_Click(object sender, EventArgs e)
        {
            SetOptions(NiFindOptions.Replace, NiFindOptions.ActionMask);
        }

        private void ShowControl(Control control, bool visible)
        {
            Padding margin;
            bool hidden = _margins.TryGetValue(control, out margin);

            if (visible == !hidden)
                return;

            if (visible)
            {
                _margins.Remove(control);
                control.Margin = margin;
                control.Visible = true;
            }
            else
            {
                _margins[control] = control.Margin;
                control.Visible = false;
                control.Margin = new Padding();
            }
        }

        private void _replaceFindNext_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.FindNext);
        }

        private void _replace_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.Replace);
        }

        private void _findPrevious_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.FindPrevious);
        }

        private void _findNext_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.FindNext);
        }

        private void _skipFile_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.SkipFile);
        }

        private void _replaceAll_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.ReplaceAll);
        }

        private void _findAll_Click(object sender, EventArgs e)
        {
            PerformFind(FindAction.FindAll);
        }

        private void PerformFind(FindAction action)
        {
            _findManager.PerformFind(action);
        }

        private void NoMoreOccurrences()
        {
            this.CreateTaskDialog()
                .MainInstruction(Labels.NoMoreOccurrences)
                .MainIcon(NiTaskDialogIcon.Information)
                .Alert(this);
        }

        private void _includeSubFolders_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.SubFolders, _includeSubFolders.Checked);
        }

        private void SetOption(NiFindOptions option, bool value)
        {
            if (value)
                _findManager.Options |= option;
            else
                _findManager.Options &= ~option;
        }

        private void _matchCase_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.MatchCase, _matchCase.Checked);
        }

        private void _matchWholeWord_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.WholeWord, _matchWholeWord.Checked);
        }

        private void _useRegularExpressions_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.RegExp, _useRegularExpressions.Checked);
        }

        private void _keepOpen_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.KeepOpen, _keepOpen.Checked);
        }

        private void _lookIn_TextChanged(object sender, EventArgs e)
        {
            NiFindOptions options;

            if (_lookIn.Text == Labels.CurrentDocument)
                options = NiFindOptions.Document;
            else if (_lookIn.Text == Labels.AllOpenDocuments)
                options = NiFindOptions.OpenDocument;
            else if (_lookIn.Text == Labels.EntireProject)
                options = NiFindOptions.Project;
            else
                options = NiFindOptions.Files;

            _findManager.Options = (_findManager.Options & ~NiFindOptions.TargetMask) | options;

            _includeSubFolders.Enabled = _findManager.Options.HasFlag(NiFindOptions.Files);
        }

        private void _lookInBrowser_Browse(object sender, EventArgs e)
        {
            var browser = new FolderBrowser();

            if (browser.ShowDialog(this) == DialogResult.OK)
                _lookIn.Text = browser.SelectedPath;
        }

        private class View : IFindView
        {
            private readonly FindForm _form;

            public View(FindForm form)
            {
                _form = form;
            }

            public void BeginUpdate()
            {
                _form.SuspendLayout();
            }

            public void EndUpdate()
            {
                _form.ResumeLayout();

                _form.Height =
                    (_form.Height - _form.ClientSize.Height) +
                    _form._toolStrip.Height +
                    _form._clientArea.Height;
            }

            public void SetMode(FindMode findMode)
            {
                _form._modeFind.Checked = findMode == FindMode.Find;
                _form._modeFindReplace.Checked = findMode == FindMode.Replace;

                bool replace = findMode == FindMode.Replace;

                _form.ShowControl(_form._replaceWith, replace);
                _form.ShowControl(_form._replaceWithLabel, replace);
                _form.ShowControl(_form._keepOpen, replace);
                _form.ShowControl(_form._skipFile, replace);
                _form.ShowControl(_form._replaceAll, replace);
                _form.ShowControl(_form._replace, replace);
                _form.ShowControl(_form._replaceFindNext, replace);
                _form.ShowControl(_form._findNext, !replace);
                _form.ShowControl(_form._findPrevious, !replace);
                _form.ShowControl(_form._findAll, !replace);
            }

            public void SetTarget(FindTarget findTarget)
            {
                string selectLookIn;

                switch (findTarget)
                {
                    case FindTarget.CurrentDocument: selectLookIn = Labels.CurrentDocument; break;
                    case FindTarget.AllOpenDocuments: selectLookIn = Labels.AllOpenDocuments; break;
                    default: selectLookIn = Labels.EntireProject; break;
                }

                _form._lookIn.SelectedItem = selectLookIn;
            }

            public void SetIncludeSubFolders(bool value)
            {
                _form._includeSubFolders.Checked = value;
            }

            public void SetMatchCase(bool value)
            {
                _form._matchCase.Checked = value;
            }

            public void SetMatchWholeWord(bool value)
            {
                _form._matchWholeWord.Checked = value;
            }

            public void SetUseRegularExpressions(bool value)
            {
                _form._useRegularExpressions.Checked = value;
            }

            public void SetKeepOpen(bool value)
            {
                _form._keepOpen.Checked = value;
            }

            public void NoMoreOccurrences()
            {
                _form.NoMoreOccurrences();
            }

            public void LoadFindWhatHistory(string[] history)
            {
                LoadHistory(_form._findWhat, history);
            }

            public void LoadReplaceWithHistory(string[] history)
            {
                LoadHistory(_form._replaceWith, history);
            }

            public void LoadLookInHistory(string[] history)
            {
                LoadHistory(_form._lookIn, history);
            }

            public void LoadLookAtFileTypesHistory(string[] history)
            {
                LoadHistory(_form._lookInFileTypes, history);
            }

            public string[] GetFindWhatHistory()
            {
                return GetHistory(_form._findWhat);
            }

            public string[] GetReplaceWithHistory()
            {
                return GetHistory(_form._replaceWith);
            }

            public string[] GetLookInHistory()
            {
                return GetHistory(_form._lookIn, DefaultLookInOptions);
            }

            public string[] GetLookAtFileTypesHistory()
            {
                return GetHistory(_form._lookInFileTypes);
            }

            private string[] GetHistory(ComboBox comboBox)
            {
                return GetHistory(comboBox, null);
            }

            private string[] GetHistory(ComboBox comboBox, string[] exclude)
            {
                var history = comboBox.Items.Cast<string>().ToList();

                if (comboBox.Text.Length > 0)
                {
                    int index = history.IndexOf(comboBox.Text);

                    if (index != -1)
                        history.RemoveAt(index);

                    history.Insert(0, comboBox.Text);
                }

                if (exclude != null)
                    history.RemoveAll(p => Array.IndexOf(exclude, p) != -1);

                if (history.Count > 20)
                    history.RemoveRange(20, history.Count - 20);

                return history.ToArray();
            }

            public string GetFindWhatText()
            {
                return _form._findWhat.Text;
            }

            public string GetReplaceWithText()
            {
                return _form._replaceWith.Text;
            }

            public string GetLookInText()
            {
                return _form._lookIn.Text;
            }

            public string GetLookAtFileTypesText()
            {
                return _form._lookInFileTypes.Text;
            }

            public object GetService(Type serviceType)
            {
                return _form.Site.GetService(serviceType);
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
        }
    }
}
