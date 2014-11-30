using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class SideBySideMarkerMapControl : Control
    {
        private SideBySideMarker[] _markers;
        private int _lines;
        private Pen[] _pixelMap;
        private Pen[] _pens;
        private bool _disposed;
        private int? _downLine;
        private double _visibleRangeStart;
        private double _visibleRangeEnd;

        public event SideBySideLineClickedEventHandler LineClicked;

        protected virtual void OnLineClicked(SideBySideLineClickedEventArgs e)
        {
            var ev = LineClicked;
            if (ev != null)
                ev(this, e);
        }

        public SideBySideMarkerMapControl()
        {
            DoubleBuffered = true;

            _pens = new Pen[3];
            _pens[(int)SideBySideMarkerType.Added] = new Pen(Color.Green);
            _pens[(int)SideBySideMarkerType.Removed] = new Pen(Color.Red);
            _pens[(int)SideBySideMarkerType.Changed] = new Pen(Color.Blue);
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

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_pens != null)
                {
                    foreach (var pen in _pens)
                    {
                        pen.Dispose();
                    }

                    _pens = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        public void SetMarkers(IEnumerable<SideBySideMarker> markers, int lines)
        {
            _markers = markers.ToArray();
            _lines = lines;

            CalculateMarkers();
            Invalidate();
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

            int height = ClientSize.Height - Padding.Vertical;
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
            int width = ClientSize.Width - Padding.Horizontal - 3;

            int start = Math.Max(e.ClipRectangle.Top, Padding.Top);
            int end = Math.Min(e.ClipRectangle.Bottom, Height - Padding.Bottom);

            for (int i = start; i < end; i++)
            {
                var pen = _pixelMap[i - start];
                if (pen != null)
                    e.Graphics.DrawLine(pen, offset, i, width, i);
            }

            int height = ClientSize.Height - Padding.Vertical;
            int boxStart = Padding.Top + (int)(_visibleRangeStart * height);
            int boxEnd = Padding.Top + (int)(_visibleRangeEnd * height);

            e.Graphics.DrawRectangle(
                SystemPens.ControlDark,
                offset - 1,
                boxStart - 1,
                width - 1,
                Math.Min(boxEnd + 2, ClientSize.Height) - boxStart
            );
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _downLine = GetLineFromOffset(e.Y);
            Capture = true;
            OnLineClicked(new SideBySideLineClickedEventArgs(_downLine.Value));
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_downLine.HasValue)
            {
                int line = GetLineFromOffset(e.Y);
                if (line != _downLine)
                {
                    _downLine = line;
                    OnLineClicked(new SideBySideLineClickedEventArgs(_downLine.Value));
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

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SETCURSOR)
            {
                if (_downLine.HasValue)
                {
                    Cursor.Current = Cursors.Hand;
                }
                else 
                {
                    var position = PointToClient(Cursor.Position);
                    if (position.Y < Padding.Top || position.Y >= (Height - Padding.Bottom))
                        Cursor.Current = Cursors.Arrow;
                    else
                        Cursor.Current = Cursors.Hand;
                }

                m.Result = (IntPtr)1;
                return;
            }

            base.WndProc(ref m);
        }

        private const int WM_SETCURSOR = 0x0020;
    }
}
