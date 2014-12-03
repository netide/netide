using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class DiffMarkerMapControl : Control
    {
        private IDiffMarker[] _markers;
        private int _lines;
        private Pen[] _pixelMap;
        private readonly Pen[] _pens;
        private int? _downLine;
        private double _visibleRangeStart;
        private double _visibleRangeEnd;

        public event DiffLineClickedEventHandler LineClicked;

        protected virtual void OnLineClicked(DiffLineClickedEventArgs e)
        {
            var ev = LineClicked;
            if (ev != null)
                ev(this, e);
        }

        protected override Cursor DefaultCursor
        {
            get { return Cursors.Hand; }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(0, 1, 0, 1); }
        }

        public DiffMarkerMapControl()
        {
            DoubleBuffered = true;

            _pens = new Pen[3];
            _pens[(int)DiffMarkerType.Added] = DiffColor.Added.DarkPen;
            _pens[(int)DiffMarkerType.Removed] = DiffColor.Removed.DarkPen;
            _pens[(int)DiffMarkerType.Changed] = DiffColor.Changed.DarkPen;
        }

        public void UpdateVisibleRange(double rangeStart, double rangeEnd)
        {
            if (rangeStart != _visibleRangeStart || rangeEnd != _visibleRangeEnd)
            {
                _visibleRangeStart = rangeStart;
                _visibleRangeEnd = rangeEnd;

                Invalidate();
                Update();
            }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);

            CalculateMarkers();
            Invalidate();
        }

        public void SetMarkers(IEnumerable<IDiffMarker> markers, int lines)
        {
            _markers = markers.ToArray();
            _lines = lines;

            CalculateMarkers();
            Invalidate();
            Update();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            CalculateMarkers();

            base.OnSizeChanged(e);
        }

        private void CalculateMarkers()
        {
            _pixelMap = null;

            if (_markers == null)
                return;

            int height = Height - Padding.Vertical;
            if (height < 0)
                return;

            _pixelMap = new Pen[height];

            foreach (var marker in _markers)
            {
                int start = (int)((marker.Line / (double)_lines) * height);
                int end = (int)(((marker.Line + marker.Length) / (double)_lines) * height);

                for (int i = start; i <= end; i++)
                {
                    _pixelMap[i] = _pens[(int)marker.Type];
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(SystemColors.Control);

            if (_pixelMap == null)
                return;

            int offset = Padding.Left + 1;
            int width = Width - Padding.Horizontal - 2;

            int start = Math.Max(e.ClipRectangle.Top, Padding.Top);
            int end = Math.Min(e.ClipRectangle.Bottom, Height - Padding.Bottom);

            for (int i = start; i < end; i++)
            {
                var pen = _pixelMap[i - start];
                if (pen != null)
                    e.Graphics.DrawLine(pen, offset, i, width, i);
            }

            int height = Height - Padding.Vertical;
            int boxStart = Padding.Top + (int)(_visibleRangeStart * height);
            int boxEnd = Padding.Top + (int)(_visibleRangeEnd * height);

            e.Graphics.DrawRectangle(
                SystemPens.ControlDark,
                offset - 1,
                boxStart - 1,
                Width - 1,
                Math.Min(boxEnd + 2, Height) - boxStart
            );
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _downLine = GetLineFromOffset(e.Y);
            Capture = true;
            OnLineClicked(new DiffLineClickedEventArgs(_downLine.Value));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_downLine.HasValue)
            {
                int line = GetLineFromOffset(e.Y);
                if (line != _downLine)
                {
                    _downLine = line;
                    OnLineClicked(new DiffLineClickedEventArgs(_downLine.Value));
                }
            }
        }

        private int GetLineFromOffset(int offset)
        {
            offset -= Padding.Top;
            int height = Height - Padding.Vertical;

            return (int)(((double)offset / height) * _lines);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_downLine.HasValue)
            {
                _downLine = null;
                Capture = false;
            }
        }
    }
}
