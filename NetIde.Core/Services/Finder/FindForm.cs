using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Settings;
using NetIde.Core.ToolWindows.FindResults;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util.Forms;

namespace NetIde.Core.Services.Finder
{
    internal partial class FindForm : DialogForm
    {
        private static readonly string[] DefaultLookInOptions = new[] { Labels.CurrentDocument, Labels.AllOpenDocuments, Labels.EntireProject };

        private readonly CorePackage _package;
        private readonly Dictionary<Control, Padding> _margins = new Dictionary<Control, Padding>();
        private INiFindHelper _findHelper;

        private FindState _state;

        public NiFindOptions Options { get; private set; }

        public FindForm(CorePackage package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            _package = package;

            InitializeComponent();
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                object obj;
                ErrorUtil.ThrowOnFailure(((INiLocalRegistry)GetService(typeof(INiLocalRegistry))).CreateInstance(
                    new Guid(NiConstants.FindHelper),
                    Site,
                    out obj
                ));

                _findHelper = (INiFindHelper)obj;

                LoadSettings();
            }
        }

        private void LoadSettings()
        {
            var settings = SettingsBuilder.GetSettings<IFinderSettings>(Site);

            LoadHistory(_findWhat, settings.FindWhatHistory);
            LoadHistory(_replaceWith, settings.ReplaceWithHistory);

            _lookIn.Items.AddRange(DefaultLookInOptions);

            LoadHistory(_lookIn, settings.LookInHistory);

            LoadHistory(_lookInFileTypes, settings.LookAtFileTypesHistory);

            SetOptions(settings.Options, settings.Options);
        }

        private void SaveSettings()
        {
            var settings = SettingsBuilder.GetSettings<IFinderSettings>(Site);

            settings.Options = Options;
            settings.FindWhatHistory = SaveHistory(_findWhat);
            settings.ReplaceWithHistory = SaveHistory(_replaceWith);
            settings.LookInHistory = SaveHistory(_lookIn, DefaultLookInOptions);
            settings.LookAtFileTypesHistory = SaveHistory(_lookInFileTypes);
        }

        private string SaveHistory(ComboBox comboBox)
        {
            return SaveHistory(comboBox, null);
        }

        private string SaveHistory(ComboBox comboBox, string[] exclude)
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

            if (history.Count > 0)
                return String.Join(Environment.NewLine, history);

            return null;
        }

        private void LoadHistory(ComboBox comboBox, string history)
        {
            if (String.IsNullOrEmpty(history))
                return;

            foreach (string line in history.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                comboBox.Items.Add(line);
            }

            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
        }

        public void SetOptions(NiFindOptions options, NiFindOptions optionsMask)
        {
            SuspendLayout();

            Options = (Options & ~optionsMask) | options;

            _modeFind.Checked = options.HasFlag(NiFindOptions.Find);
            _modeFindReplace.Checked = options.HasFlag(NiFindOptions.Replace);

            ShowControl(_replaceWith, _modeFindReplace.Checked);
            ShowControl(_replaceWithLabel, _modeFindReplace.Checked);
            ShowControl(_keepOpen, _modeFindReplace.Checked);
            ShowControl(_skipFile, _modeFindReplace.Checked);
            ShowControl(_replaceAll, _modeFindReplace.Checked);
            ShowControl(_replace, _modeFindReplace.Checked);
            ShowControl(_replaceFindNext, _modeFindReplace.Checked);
            ShowControl(_findNext, _modeFind.Checked);
            ShowControl(_findPrevious, _modeFind.Checked);
            ShowControl(_findAll, _modeFind.Checked);

            if ((optionsMask & NiFindOptions.TargetMask) != 0)
            {
                string selectLookIn;

                switch (Options & NiFindOptions.TargetMask)
                {
                    case NiFindOptions.Document: selectLookIn = Labels.CurrentDocument; break;
                    case NiFindOptions.OpenDocument: selectLookIn = Labels.AllOpenDocuments; break;
                    default: selectLookIn = Labels.EntireProject; break;
                }

                _lookIn.SelectedItem = selectLookIn;
            }

            _includeSubFolders.Checked = Options.HasFlag(NiFindOptions.SubFolders);
            _matchCase.Checked = Options.HasFlag(NiFindOptions.MatchCase);
            _matchWholeWord.Checked = Options.HasFlag(NiFindOptions.WholeWord);
            _useRegularExpressions.Checked = Options.HasFlag(NiFindOptions.RegExp);
            _keepOpen.Checked = Options.HasFlag(NiFindOptions.KeepOpen);

            ResumeLayout();

            Height =
                (Height - ClientSize.Height) +
                _toolStrip.Height +
                _clientArea.Height;
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
            PerformFind(Action.FindNext);
        }

        private void _replace_Click(object sender, EventArgs e)
        {
            PerformFind(Action.Replace);
        }

        private void _findPrevious_Click(object sender, EventArgs e)
        {
            PerformFind(Action.FindPrevious);
        }

        private void _findNext_Click(object sender, EventArgs e)
        {
            PerformFind(Action.FindNext);
        }

        private void _skipFile_Click(object sender, EventArgs e)
        {
            PerformFind(Action.SkipFile);
        }

        private void _replaceAll_Click(object sender, EventArgs e)
        {
            PerformFind(Action.ReplaceAll);
        }

        private void _findAll_Click(object sender, EventArgs e)
        {
            PerformFind(Action.FindAll);
        }

        private void PerformFind(Action action)
        {
            NiShellUtil.Checked(this, () => PerformFindChecked(action));
        }

        private void PerformFindChecked(Action action)
        {
            SaveSettings();

            var options = Options & (NiFindOptions.OptionsMask | NiFindOptions.SyntaxMark | NiFindOptions.TargetMask);

            options &= ~(NiFindOptions.ActionMask | NiFindOptions.Backwards);

            switch (action)
            {
                case Action.FindNext:
                    options |= NiFindOptions.Find;
                    break;

                case Action.FindPrevious:
                    options |= NiFindOptions.Find | NiFindOptions.Backwards;
                    break;

                case Action.Replace:
                    options |= NiFindOptions.Replace;
                    break;

                case Action.ReplaceAll:
                    options |= NiFindOptions.ReplaceAll;
                    break;

                case Action.FindAll:
                    options |= NiFindOptions.FindAll;
                    break;

                case Action.SkipFile:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException("action");
            }

            var currentFindState = FindState.Create(this, options);

            if (_state == null || !_state.SettingsEqual(currentFindState))
            {
                _state = currentFindState;
                _state.CreateIterator(this);
            }

            _state.Options = options;

            switch (action)
            {
                case Action.FindNext:
                    PerformFindSingle();
                    break;

                case Action.FindPrevious:
                    PerformFindSingle();
                    break;

                case Action.Replace:
                    PerformReplaceSingle();
                    break;

                case Action.ReplaceAll:
                    PerformReplaceAll();
                    break;

                case Action.FindAll:
                    PerformFindAll();
                    break;

                case Action.SkipFile:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException("action");
            }
        }

        private void PerformFindSingle()
        {
            if (_state.BeforeFirst)
            {
                _state.BeforeFirst = false;

                if (!_state.Iterator.MoveNext())
                {
                    NoMoreConcurrences();
                    return;
                }
            }

            do
            {
                string documentName = _state.Iterator.DocumentName;

                INiHierarchy hier;
                INiWindowFrame windowFrame;
                ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).IsDocumentOpen(
                    documentName,
                    out hier,
                    out windowFrame
                ));

                if (windowFrame == null)
                {
                    // Check whether the file has any match at all.

                    if (!HasAnyMatch(_state.Iterator.AllText))
                        continue;

                    // If we found a match, open the window frame and retry.

                    var activeProject = ((INiProjectManager)GetService(typeof(INiProjectManager))).ActiveProject;

                    if (activeProject != null)
                    {
                        hier = activeProject.FindByDocument(documentName);

                        if (hier != null)
                            ErrorUtil.ThrowOnFailure(activeProject.OpenItem(hier, out windowFrame));
                    }

                    // If we couldn't open the window frame, continue.

                    if (windowFrame == null)
                        continue;
                }

                // Now retry on the window frame.

                var docView = (INiWindowPane)windowFrame.GetPropertyEx(NiFrameProperty.DocView);
                if (docView != null)
                {
                    var findTarget = docView as INiFindTarget;
                    if (findTarget != null)
                    {
                        bool reset = _state.IsNewFindTarget(findTarget);

                        NiFindResult result;
                        ErrorUtil.ThrowOnFailure(findTarget.Find(
                            _state.FindWhat,
                            _state.Options,
                            reset,
                            _findHelper,
                            out result
                        ));

                        if (result == NiFindResult.Found)
                            return;
                    }
                }
            }
            while (_state.Iterator.MoveNext());

            NoMoreConcurrences();
        }

        private void NoMoreConcurrences()
        {
            ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).ShowMessageBox(
                Labels.NoMoreConcurrences,
                null,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            ));

            _state.ResetSeenFindTargets();
            _state.BeforeFirst = true;
        }

        private bool HasAnyMatch(string content)
        {
            int found;
            int matchLength;
            string replacementText;
            bool isFound;

            ErrorUtil.ThrowOnFailure(_findHelper.FindInText(
                _state.FindWhat,
                null,
                _state.Options,
                content,
                0,
                out found,
                out matchLength,
                out replacementText,
                out isFound
            ));

            return isFound;
        }

        private void PerformFindAll()
        {
            var toolWindow = (FindResultsWindow)_package.FindToolWindow(typeof(FindResultsWindow), true);

            ErrorUtil.ThrowOnFailure(toolWindow.Frame.Show());

            toolWindow.ResetResults();

            while (_state.Iterator.MoveNext())
            {
                string content = _state.Iterator.AllText;
                List<int> lineOffsets = null;
                int offset = 0;

                while (true)
                {
                    int found;
                    int matchLength;
                    string replacementText;
                    bool isFound;

                    ErrorUtil.ThrowOnFailure(_findHelper.FindInText(
                        _state.FindWhat,
                        null,
                        _state.Options,
                        content,
                        offset,
                        out found,
                        out matchLength,
                        out replacementText,
                        out isFound
                    ));

                    if (!isFound)
                        break;

                    if (lineOffsets == null)
                        lineOffsets = GetLineOffsets(content);

                    int line = lineOffsets.Count;

                    for (int i = 0; i < lineOffsets.Count; i++)
                    {
                        if (found <= lineOffsets[i])
                        {
                            line = i;
                            break;
                        }
                    }

                    string lineContent;

                    if (lineOffsets.Count == 0)
                    {
                        lineContent = content;
                    }
                    else if (line == 0)
                    {
                        lineContent = content.Substring(0, lineOffsets[0]);
                    }
                    else if (line == lineOffsets.Count)
                    {
                        lineContent = content.Substring(lineOffsets[lineOffsets.Count - 1]);
                    }
                    else
                    {
                        int lineOffset = lineOffsets[line - 1] + 1;
                        lineContent = content.Substring(lineOffset, lineOffsets[line] - lineOffset);
                    }

                    toolWindow.AddResult(
                        new FindResult(_state.Iterator.DocumentName, found, matchLength),
                        line,
                        lineContent.TrimEnd()
                    );

                    offset = found + matchLength;
                }
            }

            // FindAll doesn't re-use state.

            _state = null;
        }

        private List<int> GetLineOffsets(string content)
        {
            var result = new List<int>();

            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '\n')
                    result.Add(i);
            }

            return result;
        }

        private void PerformReplaceSingle()
        {
            throw new NotImplementedException();
        }

        private void PerformReplaceAll()
        {
            throw new NotImplementedException();
        }

        private void _includeSubFolders_CheckedChanged(object sender, EventArgs e)
        {
            SetOption(NiFindOptions.SubFolders, _includeSubFolders.Checked);
        }

        private void SetOption(NiFindOptions option, bool value)
        {
            if (value)
                Options |= option;
            else
                Options &= ~option;
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

            Options = (Options & ~NiFindOptions.TargetMask) | options;

            _includeSubFolders.Enabled = Options.HasFlag(NiFindOptions.Files);
        }

        private void _lookInBrowser_Browse(object sender, EventArgs e)
        {
            var browser = new FolderBrowser();

            if (browser.ShowDialog(this) == DialogResult.OK)
                _lookIn.Text = browser.SelectedPath;
        }

        private enum Action
        {
            FindNext,
            FindPrevious,
            Replace,
            SkipFile,
            ReplaceAll,
            FindAll
        }
    }
}
