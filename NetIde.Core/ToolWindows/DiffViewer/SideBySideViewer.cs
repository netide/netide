using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor.Document;
using NGit.Diff;
using NetIde.Core.Settings;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal partial class SideBySideViewer : NetIde.Util.Forms.UserControl, ITextViewer
    {
        private static readonly Color RedColor = Color.FromArgb(255, 200, 200);
        private static readonly Color GreenColor = Color.FromArgb(200, 255, 200);
        private static readonly Color BlueColor = Color.FromArgb(202, 194, 255);

        private StringBuilder _leftOut;
        private StringBuilder _rightOut;
        private List<TextMarker> _leftMarkers;
        private List<TextMarker> _rightMarkers;
        private List<SideBySideMarker> _markers; 
        private bool _readOnly = true;

        [Browsable(false)]
        [DefaultValue(true)]
        public bool ReadOnly
        {
            get { return _readOnly; }
            set { _readOnly = value; }
        }

        public SideBySideViewer()
        {
            InitializeComponent();

            _leftEditor.ActiveTextAreaControl.VScrollBar.Visible = false;
            _rightEditor.ActiveTextAreaControl.VScrollBar.Visible = false;

            _leftEditor.ActiveTextAreaControl.VScrollBar.ValueChanged += (s, e) => UpdateVisibleLines();
            _leftEditor.ActiveTextAreaControl.ScrollBarsAdjusted += (s, e) => UpdateVisibleLines();

            UpdateMarkerPadding();

            _leftEditor.IsReadOnly = true;
            _rightEditor.IsReadOnly = true;

            SyncTo(_leftEditor, _rightEditor);
            SyncTo(_rightEditor, _leftEditor);
        }

        private void UpdateVisibleLines()
        {
            var vScrollBar = _leftEditor.ActiveTextAreaControl.VScrollBar;

            int value = vScrollBar.Value;
            int maximum = vScrollBar.Maximum;

            double rangeStart = (double)value / maximum;
            double rangeEnd = (double)(value + vScrollBar.LargeChange) / maximum;

            _markerMap.UpdateVisibleRange(rangeStart, rangeEnd);
        }

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                var fontSettings = SettingsBuilder.GetSettings<IFontSettings>(Site);
                var codeFont = fontSettings.CodeFont ?? Constants.DefaultCodeFont;

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
            _leftOut = new StringBuilder();
            _rightOut = new StringBuilder();
            _leftMarkers = new List<TextMarker>();
            _rightMarkers = new List<TextMarker>();
            _markers = new List<SideBySideMarker>();

            Format(editList, leftText, rightText);

            _leftEditor.Document.MarkerStrategy.RemoveAll(p => true);
            _leftEditor.Text = _leftOut.ToString();
            _leftMarkers.ForEach(p => _leftEditor.Document.MarkerStrategy.AddMarker(p));
            _leftEditor.Refresh();

            _rightEditor.Document.MarkerStrategy.RemoveAll(p => true);
            _rightEditor.Text = _rightOut.ToString();
            _rightMarkers.ForEach(p => _rightEditor.Document.MarkerStrategy.AddMarker(p));
            _rightEditor.Refresh();

            // This is the algorithm ICSharpTextEditor uses to determine the
            // full size of the scroll bar.
            int visibleLines =
                _rightEditor.Document.GetVisibleLine(_rightEditor.Document.TotalNumberOfLines - 1) + 1 +
                _rightEditor.ActiveTextAreaControl.TextArea.TextView.VisibleLineCount * 2 / 3;

            _markerMap.SetMarkers(_markers, visibleLines);
            UpdateVisibleLines();

            _leftOut = null;
            _rightOut = null;
            _leftMarkers = null;
            _rightMarkers = null;
            _markers = null;
        }

        private void Format(EditList edits, Text a, Text b)
        {
            int curA = 0;
            int rightOffset = 0;

            foreach (var edit in edits)
            {
                for (int i = curA; i < edit.GetBeginA(); i++)
                {
                    WriteContextLine(a, i);
                    rightOffset++;
                }

                for (curA = edit.GetBeginA(); curA < edit.GetEndA(); curA++)
                {
                    WriteLine(_leftOut, _leftMarkers, a, curA, edit.GetType());
                }

                if (edit.GetEndB() > edit.GetBeginB())
                {
                    _markers.Add(new SideBySideMarker(
                        edit.GetType() == Edit.Type.REPLACE
                            ? SideBySideMarkerType.Changed
                            : SideBySideMarkerType.Added,
                        rightOffset,
                        edit.GetEndB() - edit.GetBeginB()
                    ));
                }

                for (int curB = edit.GetBeginB(); curB < edit.GetEndB(); curB++)
                {
                    WriteLine(_rightOut, _rightMarkers, b, curB, edit.GetType());
                    rightOffset++;
                }

                for (int i = edit.GetLengthB() - edit.GetLengthA(); i > 0; i--)
                {
                    WriteEmptyLine(_leftOut, _leftMarkers);
                }

                if (edit.GetLengthA() > edit.GetLengthB())
                    _markers.Add(new SideBySideMarker(SideBySideMarkerType.Removed, rightOffset, edit.GetLengthA() - edit.GetLengthB()));

                for (int i = edit.GetLengthA() - edit.GetLengthB(); i > 0; i--)
                {
                    WriteEmptyLine(_rightOut, _rightMarkers);
                    rightOffset++;
                }
            }

            for (; curA < a.Size(); curA++)
            {
                WriteContextLine(a, curA);
                rightOffset++;
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

        private void WriteContextLine(Text text, int line)
        {
            WriteLine(_leftOut, text, line);
            WriteLine(_rightOut, text, line);
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
                    color = GreenColor;
                    break;
                case Edit.Type.DELETE:
                    color = RedColor;
                    break;
                case Edit.Type.REPLACE:
                    color = BlueColor;
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

            _markerMap.SetMarkers(null, 0);
        }

        private void _leftDetails_ContentTypeSelected(object sender, EventArgs e)
        {
            _leftEditor.SetHighlighting(_leftDetails.ContentType);
        }

        private void _rightDetails_ContentTypeSelected(object sender, EventArgs e)
        {
            _rightEditor.SetHighlighting(_rightDetails.ContentType);
        }

        private void _leftDetails_SizeChanged(object sender, EventArgs e)
        {
            UpdateMarkerPadding();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateMarkerPadding();
        }

        private void UpdateMarkerPadding()
        {
            var editorTop = _leftEditor.PointToScreen(new Point(0, 0));
            var editorBottom = _leftEditor.PointToScreen(new Point(0, _leftEditor.Height));
            var markerTop = _markerMap.PointToScreen(new Point(0, 0));
            var markerBottom = _markerMap.PointToScreen(new Point(0, _markerMap.Height));

            _markerMap.Padding = new Padding(
                2,
                editorTop.Y - markerTop.Y,
                0,
                markerBottom.Y - editorBottom.Y
            );
        }

        private void _markerMap_LineClicked(object sender, SideBySideLineClickedEventArgs e)
        {
            _leftEditor.ActiveTextAreaControl.CenterViewOn(e.Line, -1);
        }
    }
}
