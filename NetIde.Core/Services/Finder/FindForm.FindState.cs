using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Services.Finder
{   
    partial class FindForm
    {
        private class FindState
        {
            private const NiFindOptions OptionsEqualMask = ~NiFindOptions.Backwards;

            private readonly HashSet<INiFindTarget> _findTargetsSeen = new HashSet<INiFindTarget>();

            public bool BeforeFirst { get; set; }
            public NiFindOptions Options { get; set; }
            public string FindWhat { get; private set; }
            public string ReplaceWith { get; private set; }
            public string LookIn { get; private set; }
            public string LookInFileTypes { get; private set; }

            public IteratorBase Iterator { get; private set; }

            private FindState()
            {
                BeforeFirst = true;
            }

            public static FindState Create(FindForm form, NiFindOptions options)
            {
                return new FindState
                {
                    Options = options,
                    FindWhat = form._findWhat.Text,
                    ReplaceWith = form._replaceWith.Text,
                    LookIn = form._lookIn.Text,
                    LookInFileTypes = form._lookInFileTypes.Text
                };
            }

            public void CreateIterator(IServiceProvider serviceProvider)
            {
                switch (Options & NiFindOptions.TargetMask)
                {
                    case NiFindOptions.OpenDocument: Iterator = new OpenDocumentIterator(this, serviceProvider); break;
                    case NiFindOptions.Project: Iterator = new ProjectIterator(this, serviceProvider); break;
                    case NiFindOptions.Files: Iterator = new FilesIterator(this, serviceProvider); break;
                    default: Iterator = new DocumentIterator(this, serviceProvider); break;
                }
            }

            public bool SettingsEqual(FindState other)
            {
                return
                    (Options & OptionsEqualMask) == (other.Options & OptionsEqualMask) &&
                    FindWhat == other.FindWhat &&
                    ReplaceWith == other.ReplaceWith &&
                    LookIn == other.LookIn &&
                    LookInFileTypes == other.LookInFileTypes;
            }

            public bool IsNewFindTarget(INiFindTarget findTarget)
            {
                if (findTarget == null)
                    throw new ArgumentNullException("findTarget");

                return _findTargetsSeen.Add(findTarget);
            }

            public void ResetSeenFindTargets()
            {
                _findTargetsSeen.Clear();
            }

            public abstract class IteratorBase
            {
                public string AllText { get; private set; }

                protected INiWindowFrame GetCurrentWindowFrame(IServiceProvider serviceProvider)
                {
                    var currentPane = ((INiWindowPaneSelection)serviceProvider.GetService(typeof(INiWindowPaneSelection))).ActiveDocument;

                    if (currentPane != null)
                    {
                        INiWindowFrame currentFrame;
                        ErrorUtil.ThrowOnFailure(((INiShell)serviceProvider.GetService(typeof(INiShell))).GetWindowFrameForWindowPane(
                            currentPane,
                            out currentFrame
                        ));

                        return currentFrame;
                    }

                    return null;
                }

                protected INiHierarchy GetCurrentHierarchy(IServiceProvider serviceProvider)
                {
                    INiHierarchy hier;
                    var currentFrame = GetCurrentWindowFrame(serviceProvider);

                    if (currentFrame != null)
                    {
                        hier = (INiHierarchy)currentFrame.GetPropertyEx(NiFrameProperty.Hierarchy);

                        if (hier != null)
                            return hier;
                    }

                    ErrorUtil.ThrowOnFailure(((INiProjectExplorer)serviceProvider.GetService(typeof(INiProjectExplorer))).GetSelectedHierarchy(
                        out hier
                    ));

                    if (hier != null)
                        return hier;

                    return ((INiProjectManager)serviceProvider.GetService(typeof(INiProjectManager))).ActiveProject;
                }

                public bool MoveNext()
                {
                    AllText = null;

                    while (FindNext())
                    {
                        AllText = GetAllText();

                        if (AllText != null)
                            return true;
                    }

                    return false;
                }

                protected abstract bool FindNext();

                protected abstract string GetAllText();

                public abstract string DocumentName { get; }

                protected string GetAllTextFromWindowFrame(INiWindowFrame windowFrame)
                {
                    var textLines = windowFrame.GetPropertyEx(NiFrameProperty.DocData) as INiTextLines;
                    if (textLines == null)
                    {
                        var docView = windowFrame.GetPropertyEx(NiFrameProperty.DocView) as INiWindowPane;
                        if (docView != null)
                        {
                            var textBufferProvider = docView as INiTextBufferProvider;
                            if (textBufferProvider != null)
                            {
                                INiTextBuffer textBuffer;
                                ErrorUtil.ThrowOnFailure(textBufferProvider.GetTextBuffer(out textBuffer));

                                textLines = textBuffer as INiTextLines;
                            }
                        }
                    }

                    if (textLines != null)
                    {
                        int line;
                        int index;
                        ErrorUtil.ThrowOnFailure(textLines.GetLastLineIndex(out line, out index));

                        string result;
                        ErrorUtil.ThrowOnFailure(textLines.GetLineText(0, 0, line, index, out result));

                        return result;
                    }

                    return null;
                }
            }

            private class ProjectIterator : IteratorBase
            {
                private readonly FindState _findState;
                private readonly IServiceProvider _serviceProvider;
                private readonly HashSet<INiHierarchy> _seen = new HashSet<INiHierarchy>();
                private INiHierarchy _current;

                public ProjectIterator(FindState findState, IServiceProvider serviceProvider)
                {
                    _findState = findState;
                    _serviceProvider = serviceProvider;
                }

                protected override bool FindNext()
                {
                    if (_current == null)
                    {
                        _current = GetCurrentHierarchy(_serviceProvider);

                        // If we're finding all items, start from the root.

                        if (
                            _findState.Options.HasFlag(NiFindOptions.FindAll) ||
                            _findState.Options.HasFlag(NiFindOptions.ReplaceAll)
                        )
                            _current = (INiHierarchy)_current.GetPropertyEx(NiHierarchyProperty.Root);

                        if (_current == null)
                            return false;
                    }
                    else
                    {
                        _current =
                            _findState.Options.HasFlag(NiFindOptions.Backwards)
                            ? _current.FindPrevious()
                            : _current.FindNext();

                        if (_current == null || _seen.Contains(_current))
                        {
                            _current = null;
                            return false;
                        }
                    }

                    _seen.Add(_current);

                    return true;
                }

                protected override string GetAllText()
                {
                    string documentName = DocumentName;

                    if (documentName == null)
                        return null;

                    INiHierarchy hier;
                    INiWindowFrame windowFrame;
                    ErrorUtil.ThrowOnFailure(((INiOpenDocumentManager)_serviceProvider.GetService(typeof(INiOpenDocumentManager))).IsDocumentOpen(
                        documentName,
                        out hier,
                        out windowFrame
                    ));

                    if (windowFrame != null)
                    {
                        Debug.Assert(hier == _current);

                        string result = GetAllTextFromWindowFrame(windowFrame);

                        if (result != null)
                            return result;
                    }

                    if (File.Exists(documentName))
                        return File.ReadAllText(documentName);

                    return null;
                }

                public override string DocumentName
                {
                    get
                    {
                        var projectItem = _current as INiProjectItem;

                        if (projectItem != null)
                        {
                            string fileName;
                            ErrorUtil.ThrowOnFailure(projectItem.GetFileName(out fileName));

                            return fileName;
                        }

                        return null;
                    }
                }
            }

            private class DocumentIterator : IteratorBase
            {
                private readonly IServiceProvider _serviceProvider;
                private INiWindowFrame _current;

                public DocumentIterator(FindState findState, IServiceProvider serviceProvider)
                {
                    _serviceProvider = serviceProvider;
                }

                protected override bool FindNext()
                {
                    _current =
                        _current == null
                        ? GetCurrentWindowFrame(_serviceProvider)
                        : null;

                    return _current != null;
                }

                protected override string GetAllText()
                {
                    return GetAllTextFromWindowFrame(_current);
                }

                public override string DocumentName
                {
                    get { return (string)_current.GetPropertyEx(NiFrameProperty.Document); }
                }
            }

            private class OpenDocumentIterator : IteratorBase
            {
                private readonly IServiceProvider _serviceProvider;
                private INiWindowFrame _current;
                private readonly HashSet<INiWindowFrame> _seen = new HashSet<INiWindowFrame>();

                public OpenDocumentIterator(FindState findState, IServiceProvider serviceProvider)
                {
                    _serviceProvider = serviceProvider;
                }

                protected override bool FindNext()
                {
                    if (_current == null)
                    {
                        _current = GetCurrentWindowFrame(_serviceProvider);
                    }
                    else
                    {
                        var allOpen = ((INiShell)_serviceProvider.GetService(typeof(INiShell))).GetDocumentWindows().ToArray();

                        int index = Array.IndexOf(allOpen, _current);

                        if (index == -1 || allOpen.Length == 1)
                            _current = null;
                        else if (index == allOpen.Length - 1)
                            _current = allOpen[0];
                        else
                            _current = allOpen[index + 1];
                    }

                    if (_seen.Contains(_current))
                        _current = null;

                    if (_current != null)
                        _seen.Add(_current);

                    return _current != null;
                }

                protected override string GetAllText()
                {
                    return GetAllTextFromWindowFrame(_current);
                }

                public override string DocumentName
                {
                    get { return (string)_current.GetPropertyEx(NiFrameProperty.Document); }
                }
            }

            private class FilesIterator : IteratorBase
            {
                private readonly List<string> _files = new List<string>();
                private int _offset = -1;

                public override string DocumentName
                {
                    get { return _files[_offset]; }
                }

                public FilesIterator(FindState findState, IServiceProvider serviceProvider)
                {
                    if (!Directory.Exists(findState.LookIn))
                        return;

                    Regex pattern = null;

                    if (!String.IsNullOrEmpty(findState.LookInFileTypes))
                    {
                        pattern = BuildPattern(
                            findState.LookInFileTypes
                                .Split(';')
                                .Select(p => p.Trim())
                                .Where(p => p.Length > 0)
                                .ToArray()
                        );
                    }

                    GetPaths(findState.LookIn, pattern, findState.Options.HasFlag(NiFindOptions.SubFolders));
                }

                private Regex BuildPattern(string[] patterns)
                {
                    if (patterns.Length == 0)
                        return null;

                    var sb = new StringBuilder("^");
                    bool hadOne = false;

                    foreach (string pattern in patterns)
                    {
                        if (hadOne)
                            sb.Append('|');
                        else
                            hadOne = true;

                        sb.Append('(');

                        foreach (char c in pattern)
                        {
                            sb.Append(c == '*' ? ".*?" : Regex.Escape(new string(c, 1)));
                        }

                        sb.Append(')');
                    }

                    sb.Append('$');

                    return new Regex(sb.ToString(), RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }

                private void GetPaths(string path, Regex pattern, bool recursive)
                {
                    foreach (string subPath in Directory.EnumerateFiles(path))
                    {
                        if (
                            !new FileInfo(subPath).Attributes.HasFlag(FileAttributes.Hidden) &&
                            (pattern == null || pattern.IsMatch(Path.GetFileName(subPath)))
                        )
                            _files.Add(subPath);
                    }

                    if (recursive)
                    {
                        foreach (string subPath in Directory.EnumerateDirectories(path))
                        {
                            if (!new FileInfo(subPath).Attributes.HasFlag(FileAttributes.Hidden))
                                GetPaths(subPath, pattern, true);
                        }
                    }
                }

                protected override bool FindNext()
                {
                    _offset++;

                    if (_offset >= _files.Count)
                    {
                        _offset = -1;
                        return false;
                    }

                    return true;
                }

                protected override string GetAllText()
                {
                    return File.ReadAllText(_files[_offset]);
                }
            }
        }
    }
}
