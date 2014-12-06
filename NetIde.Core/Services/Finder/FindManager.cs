using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Settings;
using NetIde.Core.ToolWindows.FindResults;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util;

namespace NetIde.Core.Services.Finder
{
    internal partial class FindManager
    {
        private readonly IFindView _view;
        private readonly INiFindHelper _findHelper;
        private FindState _state;

        public NiFindOptions Options { get; set; }

        public INiFindTarget FindTarget { get; set; }

        public FindManager(IFindView view)
        {
            if (view == null)
                throw new ArgumentNullException("view");

            _view = view;

            object obj;
            ErrorUtil.ThrowOnFailure(((INiLocalRegistry)_view.GetService(typeof(INiLocalRegistry))).CreateInstance(
                new Guid(NiConstants.FindHelper),
                _view,
                out obj
            ));

            _findHelper = (INiFindHelper)obj;

            LoadSettings();
        }

        private void LoadSettings()
        {
            var settings = SettingsBuilder.GetSettings<IFinderSettings>(_view);

            _view.LoadFindWhatHistory(ParseHistory(settings.FindWhatHistory));
            _view.LoadReplaceWithHistory(ParseHistory(settings.ReplaceWithHistory));
            _view.LoadLookInHistory(ParseHistory(settings.LookInHistory));
            _view.LoadLookAtFileTypesHistory(ParseHistory(settings.LookAtFileTypesHistory));

            SetOptions(settings.Options, settings.Options);
        }

        public void SaveSettings()
        {
            var settings = SettingsBuilder.GetSettings<IFinderSettings>(_view);

            settings.Options = Options;
            settings.FindWhatHistory = SerializeHistory(_view.GetFindWhatHistory());
            settings.ReplaceWithHistory = SerializeHistory(_view.GetReplaceWithHistory());
            settings.LookInHistory = SerializeHistory(_view.GetLookInHistory());
            settings.LookAtFileTypesHistory = SerializeHistory(_view.GetLookAtFileTypesHistory());
        }

        private string SerializeHistory(string[] history)
        {
            return String.Join(Environment.NewLine, history);
        }

        private string[] ParseHistory(string history)
        {
            if (String.IsNullOrEmpty(history))
                return ArrayUtil.GetEmptyArray<string>();

            return history.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public void SetOptions(NiFindOptions options, NiFindOptions optionsMask)
        {
            _view.BeginUpdate();

            Options = (Options & ~optionsMask) | options;

            bool replace = options.HasFlag(NiFindOptions.Replace);

            _view.SetMode(replace ? FindMode.Replace : FindMode.Find);

            if ((optionsMask & NiFindOptions.TargetMask) != 0)
            {
                switch (Options & NiFindOptions.TargetMask)
                {
                    case NiFindOptions.Document: _view.SetTarget(Finder.FindTarget.CurrentDocument); break;
                    case NiFindOptions.OpenDocument: _view.SetTarget(Finder.FindTarget.AllOpenDocuments); break;
                    default: _view.SetTarget(Finder.FindTarget.EntireProject); break;
                }
            }

            _view.SetIncludeSubFolders(Options.HasFlag(NiFindOptions.SubFolders));
            _view.SetMatchCase(Options.HasFlag(NiFindOptions.MatchCase));
            _view.SetMatchWholeWord(Options.HasFlag(NiFindOptions.WholeWord));
            _view.SetUseRegularExpressions(Options.HasFlag(NiFindOptions.RegExp));
            _view.SetKeepOpen(Options.HasFlag(NiFindOptions.KeepOpen));

            _view.EndUpdate();
        }

        public void PerformFind(FindAction action)
        {
            NiShellUtil.Checked(_view, () => PerformFindChecked(action));
        }

        private void PerformFindChecked(FindAction action)
        {
            SaveSettings();

            var options = Options & (NiFindOptions.OptionsMask | NiFindOptions.SyntaxMark | NiFindOptions.TargetMask);

            options &= ~(NiFindOptions.ActionMask | NiFindOptions.Backwards);

            switch (action)
            {
                case FindAction.FindNext:
                    options |= NiFindOptions.Find;
                    break;

                case FindAction.FindPrevious:
                    options |= NiFindOptions.Find | NiFindOptions.Backwards;
                    break;

                case FindAction.Replace:
                    options |= NiFindOptions.Replace;
                    break;

                case FindAction.ReplaceAll:
                    options |= NiFindOptions.ReplaceAll;
                    break;

                case FindAction.FindAll:
                    options |= NiFindOptions.FindAll;
                    break;

                case FindAction.SkipFile:
                    throw new NotImplementedException();

                default:
                    throw new ArgumentOutOfRangeException("action");
            }

            var currentFindState = FindState.Create(this, options, FindTarget);

            if (_state == null || !_state.SettingsEqual(currentFindState))
            {
                _state = currentFindState;
                _state.CreateIterator(_view);
            }

            _state.Options = options;

            switch (action)
            {
                case FindAction.FindNext:
                    PerformFindSingle();
                    break;

                case FindAction.FindPrevious:
                    PerformFindSingle();
                    break;

                case FindAction.Replace:
                    PerformReplaceSingle();
                    break;

                case FindAction.ReplaceAll:
                    PerformReplaceAll();
                    break;

                case FindAction.FindAll:
                    PerformFindAll();
                    break;

                case FindAction.SkipFile:
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
                    NoMoreOccurrences();
                    return;
                }
            }

            if (FindTarget != null)
            {
                PerformSingleFind(FindTarget);
                return;
            }

            do
            {
                string documentName = _state.Iterator.DocumentName;

                INiHierarchy hier;
                INiWindowFrame windowFrame;
                ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)_view.GetService(typeof(INiOpenDocumentManager))).IsDocumentOpen(
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

                    var activeProject = ((INiProjectManager)_view.GetService(typeof(INiProjectManager))).ActiveProject;

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
                    if (findTarget != null && PerformSingleFind(findTarget))
                        return;
                }
            }
            while (_state.Iterator.MoveNext());

            NoMoreOccurrences();
        }

        private void NoMoreOccurrences()
        {
            _view.NoMoreOccurrences();

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

        private bool PerformSingleFind(INiFindTarget findTarget)
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
                return true;
            return false;
        }

        private void PerformFindAll()
        {
            var corePackage = ((NiFinder)_view.GetService(typeof(INiFinder))).CorePackage;
            var toolWindow = (FindResultsWindow)corePackage.FindToolWindow(typeof(FindResultsWindow), true);

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
    }
}
