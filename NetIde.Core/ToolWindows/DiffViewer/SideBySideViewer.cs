using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using NetIde.Shell;
using NetIde.Util.Forms;
using NGit.Diff;
using NetIde.Core.Settings;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util;
using TextEditorControl = NetIde.Core.TextEditor.TextEditorControl;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class SideBySideViewer : NetIde.Util.Forms.UserControl, ITextViewer
    {
        private List<IDiffMarker> _markers;
        private int _visibleLines;
        private bool _readOnly = true;
        private bool _designing;

        [Browsable(false)]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                if (_readOnly != value)
                {
                    _readOnly = value;

                    if (_readOnly)
                        _editor.SetMarkers(null, 0);
                    else
                        _editor.SetMarkers(_markers, _visibleLines);
                }
            }
        }

        public event EventHandler LeftUpdated;

        protected virtual void OnLeftUpdated(EventArgs e)
        {
            var ev = LeftUpdated;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler RightUpdated;

        protected virtual void OnRightUpdated(EventArgs e)
        {
            var ev = RightUpdated;
            if (ev != null)
                ev(this, e);
        }

        public SideBySideViewer()
        {
            _designing = ControlUtil.GetIsInDesignMode(this);

            InitializeComponent();

            _editor.Margin = new Padding(
                0,
                1,
                0,
                1 + _leftEditor.ActiveTextAreaControl.HScrollBar.Height
            );

            _markerMap.Margin = new Padding(
                2,
                0,
                2,
                _leftEditor.ActiveTextAreaControl.HScrollBar.Height
            );

            _leftEditor.ActiveTextAreaControl.VScrollBar.Visible = false;
            _rightEditor.ActiveTextAreaControl.VScrollBar.Visible = false;

            _leftEditor.ActiveTextAreaControl.VScrollBar.ValueChanged += (s, e) => UpdateVisibleLines();
            _leftEditor.ActiveTextAreaControl.ScrollBarsAdjusted += (s, e) => UpdateVisibleLines();

            _leftEditor.IsReadOnly = true;
            _rightEditor.IsReadOnly = true;

            _leftEditor.CommandMapper.Add(
                NiResources.TextEditor_Copy,
                p => new TrimmedCopy(this, _leftEditor).Execute(_leftEditor.ActiveTextAreaControl.TextArea),
                p => p.Status = _leftEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );
            _rightEditor.CommandMapper.Add(
                NiResources.TextEditor_Copy,
                p => new TrimmedCopy(this, _rightEditor).Execute(_rightEditor.ActiveTextAreaControl.TextArea),
                p => p.Status = _rightEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected ? NiCommandStatus.Enabled : NiCommandStatus.Supported
            );

            SyncTo(_leftEditor, _rightEditor);
            SyncTo(_rightEditor, _leftEditor);
        }

        public string GetLeftText()
        {
            return GetTrimmedText(
                _leftEditor,
                0,
                _leftEditor.Document.TextLength
            );
        }

        public string GetRightText()
        {
            return GetTrimmedText(
                _rightEditor,
                0,
                _rightEditor.Document.TextLength
            );
        }

        private string GetTrimmedText(TextEditorControl editor, int start, int end)
        {
            var document = editor.Document;

            // The document contains empty lines to fill up space because the
            // other side has more text. This removes these empty lines from
            // the output.

            var sb = new StringBuilder();

            bool isLeft = editor == _leftEditor;
            int offset = start;

            foreach (var marker in _markers)
            {
                int startLength = isLeft ? marker.LeftLength : marker.RightLength;
                int endLength = marker.Length;

                if (startLength != endLength)
                {
                    int markerStart = document.PositionToOffset(new TextLocation(
                        0,
                        marker.Line + startLength
                    ));
                    int markerEnd = document.PositionToOffset(new TextLocation(
                        0,
                        marker.Line + endLength
                    ));

                    if (markerStart < offset)
                        offset = markerEnd;

                    if (markerEnd > offset)
                    {
                        sb.Append(document.GetText(
                            offset,
                            Math.Min(markerStart, end) - offset
                        ));

                        offset = markerEnd;
                    }

                    if (offset >= end)
                        break;
                }
            }

            if (offset < end)
            {
                sb.Append(document.GetText(
                    offset,
                    end - offset
                ));
            }

            return sb.ToString();
        }

        private void UpdateVisibleLines()
        {
            var vScrollBar = _leftEditor.ActiveTextAreaControl.VScrollBar;

            int value = vScrollBar.Value;
            int maximum = vScrollBar.Maximum;

            double rangeStart = (double)value / maximum;
            double rangeEnd = (double)(value + vScrollBar.LargeChange) / maximum;

            _markerMap.UpdateVisibleRange(rangeStart, rangeEnd);

            _editor.UpdateVisibleRange(
                value,
                vScrollBar.LargeChange,
                _leftEditor.ActiveTextAreaControl.TextArea.TextView.FontHeight
            );
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                if (_designing)
                    return;

                var fontSettings = SettingsBuilder.GetSettings<IFontSettings>(Site);
                var codeFont = fontSettings.CodeFont ?? Constants.DefaultCodeFont;

                _leftEditor.Site = value;
                _rightEditor.Site = value;

                _leftEditor.Font = codeFont;
                _rightEditor.Font = codeFont;
            }
        }

        private void SyncTo(TextEditorControl source, TextEditorControl target)
        {
            source.ActiveTextAreaControl.HScrollBar.ValueChanged += (s, e) =>
            {
                target.ActiveTextAreaControl.TextArea.VirtualTop =
                    source.ActiveTextAreaControl.TextArea.VirtualTop;
                source.ActiveTextAreaControl.TextArea.Update();
                target.ActiveTextAreaControl.TextArea.Update();
            };
            source.ActiveTextAreaControl.VScrollBar.ValueChanged += (s, e) =>
            {
                target.ActiveTextAreaControl.TextArea.VirtualTop =
                    source.ActiveTextAreaControl.TextArea.VirtualTop;
                source.ActiveTextAreaControl.TextArea.Update();
                target.ActiveTextAreaControl.TextArea.Update();
            };
        }

        public void SelectDetails(IStream leftStream, FileType leftFileType, IStream rightStream, FileType rightFileType)
        {
            _leftDetails.SelectDetails(leftStream, leftFileType);
            _rightDetails.SelectDetails(rightStream, rightFileType);

            _leftEditor.SetHighlighting(_leftDetails.ContentType);
            _rightEditor.SetHighlighting(_rightDetails.ContentType);
        }

        public void LoadDiff(Text leftText, Text rightText, EditList editList)
        {
            var leftOut = new StringBuilder();
            var rightOut = new StringBuilder();
            var leftMarkers = new List<TextMarker>();
            var rightMarkers = new List<TextMarker>();
            
            _markers = new List<IDiffMarker>();

            Format(editList, leftText, rightText, leftMarkers, rightMarkers, leftOut, rightOut);

            _leftEditor.Document.MarkerStrategy.RemoveAll(p => true);
            _leftEditor.Text = leftOut.ToString();
            leftMarkers.ForEach(p => _leftEditor.Document.MarkerStrategy.AddMarker(p));
            _leftEditor.Refresh();

            _rightEditor.Document.MarkerStrategy.RemoveAll(p => true);
            _rightEditor.Text = rightOut.ToString();
            rightMarkers.ForEach(p => _rightEditor.Document.MarkerStrategy.AddMarker(p));
            _rightEditor.Refresh();

            // This is the algorithm ICSharpTextEditor uses to determine the
            // full size of the scroll bar.
            _visibleLines =
                _rightEditor.Document.GetVisibleLine(_rightEditor.Document.TotalNumberOfLines - 1) + 1 +
                _rightEditor.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount * 2 / 3;

            _markerMap.SetMarkers(_markers, _visibleLines);
            if (!_readOnly)
                _editor.SetMarkers(_markers, _visibleLines);
            UpdateVisibleLines();
        }

        private void Format(EditList edits, Text a, Text b, List<TextMarker> leftMarkers, List<TextMarker> rightMarkers, StringBuilder leftOut, StringBuilder rightOut)
        {
            int curA = 0;
            int offset = 0;

            foreach (var edit in edits)
            {
                for (int i = curA; i < edit.GetBeginA(); i++)
                {
                    WriteContextLine(a, i, leftOut, rightOut);
                    offset++;
                }

                _markers.Add(new DiffMarker(
                    GetMarkerType(edit.GetType()),
                    offset,
                    Math.Max(edit.GetLengthA(), edit.GetLengthB()),
                    edit.GetLengthA(),
                    edit.GetLengthB()
                ));

                for (curA = edit.GetBeginA(); curA < edit.GetEndA(); curA++)
                {
                    WriteLine(leftOut, leftMarkers, a, curA, edit.GetType());
                }

                for (int curB = edit.GetBeginB(); curB < edit.GetEndB(); curB++)
                {
                    WriteLine(rightOut, rightMarkers, b, curB, edit.GetType());
                    offset++;
                }

                for (int i = edit.GetLengthB() - edit.GetLengthA(); i > 0; i--)
                {
                    WriteEmptyLine(leftOut, leftMarkers);
                }

                for (int i = edit.GetLengthA() - edit.GetLengthB(); i > 0; i--)
                {
                    WriteEmptyLine(rightOut, rightMarkers);
                    offset++;
                }
            }

            for (; curA < a.Size(); curA++)
            {
                WriteContextLine(a, curA, leftOut, rightOut);
                offset++;
            }
        }

        private DiffMarkerType GetMarkerType(Edit.Type type)
        {
            switch (type)
            {
                case Edit.Type.INSERT:
                    return DiffMarkerType.Added;
                case Edit.Type.DELETE:
                    return DiffMarkerType.Removed;
                case Edit.Type.REPLACE:
                    return DiffMarkerType.Changed;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void WriteEmptyLine(StringBuilder @out, List<TextMarker> markers)
        {
            int start = @out.Length;
            @out.AppendLine();
            int end = @out.Length;

            markers.Add(new TextMarker(
                start,
                end - start,
                TextMarkerType.SolidBlock | TextMarkerType.ExtendToBorder,
                _leftEditor.BackColor,
                Color.FromArgb(179, 179, 179),
                HatchStyle.LightUpwardDiagonal
            ));
        }

        private void WriteContextLine(Text text, int line, StringBuilder leftOut, StringBuilder rightOut)
        {
            WriteLine(leftOut, text, line);
            WriteLine(rightOut, text, line);
        }

        private void WriteLine(StringBuilder @out, List<TextMarker> markers, Text text, int line, Edit.Type editType)
        {
            int start = @out.Length;
            WriteLine(@out, text, line);
            int end = @out.Length;

            Color color;

            switch (editType)
            {
                case Edit.Type.INSERT:
                    color = DiffColor.Added.LightColor;
                    break;
                case Edit.Type.DELETE:
                    color = DiffColor.Removed.LightColor;
                    break;
                case Edit.Type.REPLACE:
                    color = DiffColor.Changed.LightColor;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("editType");
            }

            markers.Add(new TextMarker(
                start,
                end - start,
                TextMarkerType.SolidBlock | TextMarkerType.ExtendToBorder,
                color
            ));
        }

        private void WriteLine(StringBuilder @out, Text text, int cur)
        {
            text.WriteLine(@out, cur);
            @out.Append('\n');
        }

        public void Reset()
        {
            _leftEditor.Text = null;
            _leftEditor.Refresh();
            _rightEditor.Text = null;
            _rightEditor.Refresh();
            _markers = null;
            _visibleLines = 0;

            _markerMap.SetMarkers(null, 0);
            _editor.SetMarkers(null, 0);
            UpdateVisibleLines();
        }

        private void _leftDetails_ContentTypeSelected(object sender, EventArgs e)
        {
            _leftEditor.SetHighlighting(_leftDetails.ContentType);
        }

        private void _rightDetails_ContentTypeSelected(object sender, EventArgs e)
        {
            _rightEditor.SetHighlighting(_rightDetails.ContentType);
        }

        private void _markerMap_LineClicked(object sender, DiffLineClickedEventArgs e)
        {
            _leftEditor.ActiveTextAreaControl.CenterViewOn(e.Line, -1);
        }

        private void _editor_ButtonClick(object sender, DiffEditorButtonEventArgs e)
        {
            Debug.Assert(!_readOnly);

            switch (e.Type)
            {
                case DiffEditorButtonType.CopyLeft:
                    PerformCopy(
                        _leftEditor,
                        _rightEditor,
                        e.Marker.Line,
                        e.Marker.Length,
                        e.Marker.LeftLength
                    );
                    RemoveMarker(e.Marker, e.Marker.Length - e.Marker.LeftLength);
                    break;

                case DiffEditorButtonType.DeleteLeft:
                case DiffEditorButtonType.DeleteRight:
                    RemoveFromDocument(_leftEditor, e.Marker.Line, e.Marker.Length);
                    RemoveFromDocument(_rightEditor, e.Marker.Line, e.Marker.Length);
                    RemoveMarker(e.Marker, e.Marker.Length);
                    break;

                case DiffEditorButtonType.CopyRight:
                    PerformCopy(
                        _rightEditor,
                        _leftEditor,
                        e.Marker.Line,
                        e.Marker.Length,
                        e.Marker.RightLength
                    );
                    RemoveMarker(e.Marker, e.Marker.Length - e.Marker.RightLength);
                    break;
            }

            switch (e.Type)
            {
                case DiffEditorButtonType.CopyLeft:
                case DiffEditorButtonType.DeleteRight:
                    OnRightUpdated(EventArgs.Empty);
                    break;

                case DiffEditorButtonType.DeleteLeft:
                case DiffEditorButtonType.CopyRight:
                    OnLeftUpdated(EventArgs.Empty);
                    break;
            }
        }

        private void RemoveMarker(IDiffMarker marker, int removeLength)
        {
            int index = _markers.IndexOf(marker);
            _markers.RemoveAt(index);

            if (removeLength > 0)
            {
                _visibleLines -= removeLength;

                for (int i = index; i < _markers.Count; i++)
                {
                    ((DiffMarker)_markers[i]).Line -= removeLength;
                }
            }

            _markerMap.SetMarkers(_markers, _visibleLines);
            if (!_readOnly)
                _editor.SetMarkers(_markers, _visibleLines);
        }

        private void PerformCopy(TextEditorControl from, TextEditorControl to, int line, int length, int fromLength)
        {
            var fromDocument = from.Document;

            // Get the text we're going to copy.

            int fromStart = fromDocument.PositionToOffset(new TextLocation(
                0,
                line
            ));
            int fromEnd = fromDocument.PositionToOffset(new TextLocation(
                0,
                line + fromLength
            ));
            string fromText = fromDocument.GetText(fromStart, fromEnd - fromStart);

            UpdateEditor(from, () =>
            {
                // Remove the markers.

                int start = from.Document.PositionToOffset(new TextLocation(
                    0,
                    line
                ));
                int end = from.Document.PositionToOffset(new TextLocation(
                    0,
                    line + length
                ));

                from.Document.MarkerStrategy.RemoveAll(p => p.Offset >= start && p.EndOffset <= end);

                // Remove empty lines from the source editor.

                if (fromLength != length)
                {
                    start = from.Document.PositionToOffset(new TextLocation(
                        0,
                        line + fromLength
                    ));
                    from.Document.Remove(start, end - start);
                }
            });

            UpdateEditor(to, () =>
            {
                // Update the target.

                int start = to.Document.PositionToOffset(new TextLocation(
                    0,
                    line
                ));
                int end = to.Document.PositionToOffset(new TextLocation(
                    0,
                    line + length
                ));
                to.Document.Replace(start, end - start, fromText);
            });
        }

        private void RemoveFromDocument(TextEditorControl editor, int line, int length)
        {
            UpdateEditor(editor, () =>
            {
                var start = editor.Document.PositionToOffset(new TextLocation(
                    0, line
                ));
                var end = editor.Document.PositionToOffset(new TextLocation(
                    0, line + length
                ));

                editor.Document.Remove(start, end - start);
            });
        }

        private void UpdateEditor(TextEditorControl editor, Action action)
        {
            var textArea = editor.ActiveTextAreaControl.TextArea;
            var document = textArea.Document;

            document.ReadOnly = false;

            try
            {
                textArea.BeginUpdate();

                action();

                textArea.Caret.ValidateCaretPos();
                textArea.EndUpdate();
                textArea.Refresh();
            }
            finally
            {
                document.ReadOnly = false;
            }
        }

        private class DiffMarker : IDiffMarker
        {
            public DiffMarkerType Type { get; private set; }
            public int Line { get; set; }
            public int Length { get; private set; }
            public int LeftLength { get; private set; }
            public int RightLength { get; private set; }

            public DiffMarker(DiffMarkerType type, int line, int length, int leftLength, int rightLength)
            {
                Type = type;
                Line = line;
                Length = length;
                RightLength = rightLength;
                LeftLength = leftLength;
            }
        }

        private class TrimmedCopy : AbstractEditAction
        {
            private const string LineSelectedType = "MSDEVLineSelect";  // This is the type VS 2003 and 2005 use for flagging a whole line copy
            
            private readonly SideBySideViewer _owner;
            private readonly TextEditorControl _editor;
            private static int _safeSetClipboardDataVersion;

            public TrimmedCopy(SideBySideViewer owner, TextEditorControl editor)
            {
                _owner = owner;
                _editor = editor;
            }

            public override void Execute(TextArea textArea)
            {
                textArea.AutoClearSelection = false;

                int start;
                int end;
                bool asLine;

                if (textArea.SelectionManager.HasSomethingSelected)
                {
                    var selection = textArea.SelectionManager.SelectionCollection[0];

                    start = selection.Offset;
                    end = selection.EndOffset;
                    asLine = false;
                }
                else if (textArea.Document.TextEditorProperties.CutCopyWholeLine)
                {
                    int curLineNr = textArea.Document.GetLineNumberForOffset(textArea.Caret.Offset);
                    var lineWhereCaretIs = textArea.Document.GetLineSegment(curLineNr);
                    start = lineWhereCaretIs.Offset;
                    end = start + lineWhereCaretIs.TotalLength;
                    asLine = true;
                }
                else
                {
                    return;
                }

                string selectedText = _owner.GetTrimmedText(_editor, start, end);

                if (selectedText.Length > 0)
                    CopyTextToClipboard(selectedText, asLine);
            }

            private void CopyTextToClipboard(string stringToCopy, bool asLine)
            {
                var dataObject = new DataObject();
                dataObject.SetData(DataFormats.UnicodeText, true, stringToCopy);

                if (asLine)
                {
                    var lineSelected = new MemoryStream(1);
                    lineSelected.WriteByte(1);
                    dataObject.SetData(LineSelectedType, false, lineSelected);
                }

                SafeSetClipboard(dataObject);
            }

            static void SafeSetClipboard(object dataObject)
            {
                // Work around ExternalException bug. (SD2-426)
                // Best reproducable inside Virtual PC.
                int version = unchecked(++_safeSetClipboardDataVersion);

                try
                {
                    Clipboard.SetDataObject(dataObject, true);
                }
                catch (ExternalException)
                {
                    var timer = new Timer();
                    timer.Interval = 100;
                    timer.Tick += delegate
                    {
                        timer.Stop();
                        timer.Dispose();

                        if (_safeSetClipboardDataVersion != version)
                            return;

                        try
                        {
                            Clipboard.SetDataObject(dataObject, true, 10, 50);
                        }
                        catch (ExternalException)
                        {
                        }
                    };

                    timer.Start();
                }
            }
        }
    }
}
