using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class DiffEditorControl : Control
    {
        private const int DisabledWidth = 4;
        private const int ButtonWidth = 9;
        private const int SeparatorWidth = 9;
        private static readonly Image RaquoImage = NeutralResources.DiffRaquo;
        private static readonly Image LaquoImage = NeutralResources.DiffLaquo;
        private static readonly Image CloseImage = NeutralResources.DiffClose;

        private Marker[] _markers;
        private ViewPort _viewPort;
        private MarkerButton _downButton;

        public event DiffEditorButtonEventHandler ButtonClick;

        protected virtual void OnButtonClick(DiffEditorButtonEventArgs e)
        {
            var ev = ButtonClick;
            if (ev != null)
                ev(this, e);
        }

        public DiffEditorControl()
        {
            DoubleBuffered = true;
        }

        public void SetMarkers(List<IDiffMarker> markers, int visibleLines)
        {
            if (markers == null)
            {
                _markers = null;
                Width = DisabledWidth;
                Invalidate();
                return;
            }

            Width = (ButtonWidth * 2 + 2) * 2 + SeparatorWidth;

            // Rebuild the marker list to have it indexed by line.

            _markers = new Marker[visibleLines];

            foreach (var editMarker in markers)
            {
                var buttons = new MarkerButton[4];

                AddButton(buttons, DiffEditorButtonType.CopyLeft, editMarker);
                AddButton(buttons, DiffEditorButtonType.CopyRight, editMarker);

                if (editMarker.Type == DiffMarkerType.Removed)
                    AddButton(buttons, DiffEditorButtonType.DeleteLeft, editMarker);
                if (editMarker.Type == DiffMarkerType.Added)
                    AddButton(buttons, DiffEditorButtonType.DeleteRight, editMarker);

                var marker = new Marker(editMarker, buttons);

                for (int i = 0; i < editMarker.Length; i++)
                {
                    Debug.Assert(_markers[editMarker.Line + i] == null);
                    _markers[editMarker.Line + i] = marker;
                }
            }

            Invalidate();
            Update();
        }

        private void AddButton(MarkerButton[] buttons, DiffEditorButtonType type, IDiffMarker editMarker)
        {
            int top = editMarker.Line * _viewPort.LineHeight;
            int height = editMarker.Length * _viewPort.LineHeight;
            int left;

            switch (type)
            {
                case DiffEditorButtonType.CopyLeft:
                    left = 0;
                    break;
                case DiffEditorButtonType.DeleteLeft:
                    left = ButtonWidth + 1;
                    break;
                case DiffEditorButtonType.DeleteRight:
                    left = Width - (ButtonWidth * 2 + 1);
                    break;
                case DiffEditorButtonType.CopyRight:
                    left = Width - ButtonWidth;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            buttons[(int)type] = new MarkerButton(type, new Rectangle(left, top, ButtonWidth, height));
        }

        public void UpdateVisibleRange(int offset, int visible, int lineHeight)
        {
            _viewPort = new ViewPort(offset, visible, lineHeight, Width);

            Invalidate();
            Update();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);

            if (_markers == null || _viewPort == null)
                return;

            int startLine = (_viewPort.Offset + e.ClipRectangle.Top) / _viewPort.LineHeight;
            int endLine = Math.Min(
                (_viewPort.Offset + e.ClipRectangle.Bottom + _viewPort.LineHeight - 1) / _viewPort.LineHeight,
                _markers.Length - 1
            );

            Marker lastMarker = null;

            for (int i = startLine; i <= endLine; i++)
            {
                var marker = _markers[i];
                if (marker == lastMarker)
                    continue;

                lastMarker = marker;

                if (marker != null)
                    marker.Draw(_viewPort, e, Height);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_viewPort == null)
                return;

            Capture = true;

            var location = e.Location;
            location.Offset(0, _viewPort.Offset);

            var button = FindButton(location);
            _downButton = button == null ? null : button.Item2;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!Capture)
                return;

            if (_downButton != null)
            {
                var location = e.Location;
                location.Offset(0, _viewPort.Offset);

                var button = FindButton(location);

                if (button != null && button.Item2 == _downButton)
                    OnButtonClick(new DiffEditorButtonEventArgs(button.Item1.EditMarker, button.Item2.Type));

                _downButton = null;
            }

            Capture = false;
        }

        private Tuple<Marker, MarkerButton> FindButton(Point location)
        {
            int line = location.Y / _viewPort.LineHeight;

            var marker = _markers[line];
            if (marker != null)
            {
                foreach (var button in marker.Buttons)
                {
                    if (button != null && button.Bounds.Contains(location))
                        return Tuple.Create(marker, button);
                }
            }

            return null;
        }

        private class ViewPort
        {
            public int Offset { get; private set; }
            public int Height { get; private set; }
            public int LineHeight { get; private set; }
            public int Width { get; private set; }

            public ViewPort(int offset, int height, int lineHeight, int width)
            {
                Offset = offset;
                Height = height;
                LineHeight = lineHeight;
                Width = width;
            }
        }

        private class Marker
        {
            public IDiffMarker EditMarker { get; private set; }
            public MarkerButton[] Buttons { get; private set; }
            public DiffColor Color { get; private set; }

            public Marker(IDiffMarker editMarker, MarkerButton[] buttons)
            {
                EditMarker = editMarker;
                Buttons = buttons;

                switch (editMarker.Type)
                {
                    case DiffMarkerType.Added:
                        Color = DiffColor.Added;
                        break;
                    case DiffMarkerType.Removed:
                        Color = DiffColor.Removed;
                        break;
                    case DiffMarkerType.Changed:
                        Color = DiffColor.Changed;
                        break;
                }
            }

            public void Draw(ViewPort viewPort, PaintEventArgs e, int height)
            {
                var bounds = new Rectangle(
                    0,
                    EditMarker.Line * viewPort.LineHeight - viewPort.Offset,
                    viewPort.Width,
                    EditMarker.Length * viewPort.LineHeight
                );

                e.Graphics.FillRectangle(
                    Color.LightBrush,
                    bounds
                );

                e.Graphics.DrawLine(
                    SystemPens.ControlDark,
                    bounds.Left,
                    bounds.Top,
                    bounds.Width - 1,
                    bounds.Top
                );

                e.Graphics.DrawLine(
                    SystemPens.ControlDark,
                    bounds.Left,
                    bounds.Bottom - 1,
                    bounds.Width - 1,
                    bounds.Bottom - 1
                );

                foreach (var button in Buttons)
                {
                    if (button != null)
                        button.Draw(viewPort, e, height);
                }
            }
        }

        private class MarkerButton
        {
            public DiffEditorButtonType Type { get; private set; }
            public Rectangle Bounds { get; private set; }
            public Image Image { get; private set; }

            public MarkerButton(DiffEditorButtonType type, Rectangle bounds)
            {
                Type = type;
                Bounds = bounds;

                switch (Type)
                {
                    case DiffEditorButtonType.CopyLeft:
                        Image = RaquoImage;
                        break;
                    case DiffEditorButtonType.DeleteLeft:
                    case DiffEditorButtonType.DeleteRight:
                        Image = CloseImage;
                        break;
                    case DiffEditorButtonType.CopyRight:
                        Image = LaquoImage;
                        break;
                }
            }

            public void Draw(ViewPort viewPort, PaintEventArgs e, int height)
            {
                var bounds = Bounds;
                bounds.Offset(0, -viewPort.Offset);

                if (Type == DiffEditorButtonType.CopyLeft || Type == DiffEditorButtonType.DeleteLeft)
                {
                    e.Graphics.DrawLine(
                        SystemPens.ControlDark,
                        bounds.Right,
                        bounds.Top + 1,
                        bounds.Right,
                        bounds.Bottom - 2
                    );
                }
                else
                {
                    e.Graphics.DrawLine(
                        SystemPens.ControlDark,
                        bounds.Left - 1,
                        bounds.Top + 1,
                        bounds.Left - 1,
                        bounds.Bottom - 2
                    );
                }

                int top = Math.Max(bounds.Top, 0);
                int bottom = Math.Min(bounds.Bottom, height);
                if (bottom - top < viewPort.LineHeight)
                    bottom = top + viewPort.LineHeight;

                e.Graphics.DrawImage(
                    Image,
                    bounds.Left + (bounds.Width - Image.Width) / 2,
                    top + ((bottom - top) - Image.Height) / 2
                );
            }
        }
    }
}
