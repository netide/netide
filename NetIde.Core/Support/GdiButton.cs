using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GdiPresentation;
using Titan;
using Brush = GdiPresentation.Brush;
using Size = GdiPresentation.Size;
using LinearGradientBrush = System.Drawing.Drawing2D.LinearGradientBrush;
using LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode;
using MouseEventArgs = GdiPresentation.MouseEventArgs;

namespace NetIde.Core.Support
{
    internal class GdiButton : Element
    {
        private static ProfessionalColorTable ColorTable
        {
            get { return ((ToolStripProfessionalRenderer)ToolStripManager.Renderer).ColorTable; }
        }

        private State _state;
        private bool _isToggle;
        private System.Drawing.Image _bitmap;
        private System.Drawing.Image _grayBitmap;
        private Thickness _padding;

        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                if (_padding != value)
                {
                    _padding = value;

                    Invalidate();
                }
            }
        }

        public bool IsToggle
        {
            get { return _isToggle; }
            set
            {
                if (_isToggle != value)
                {
                    _isToggle = value;

                    IsChecked = false;
                }
            }
        }

        public bool IsChecked
        {
            get { return (_state & State.Checked) != 0; }
            set
            {
                if (IsChecked != value)
                {
                    if (value)
                        _state |= State.Checked;
                    else
                        _state &= ~State.Checked;

                    Invalidate();

                    OnIsCheckedChanged(EventArgs.Empty);
                }
            }
        }

        public bool IsEnabled
        {
            get { return (_state & State.Enabled) != 0; }
            set
            {
                if (IsEnabled != value)
                {
                    if (value)
                        _state |= State.Enabled;
                    else
                        _state &= ~State.Enabled;

                    Focusable = value;

                    Invalidate();
                }
            }
        }

        public System.Drawing.Image Bitmap
        {
            get { return _bitmap; }
            set
            {
                if (_bitmap != value)
                {
                    _bitmap = value;

                    if (_grayBitmap != null)
                    {
                        _grayBitmap.Dispose();
                        _grayBitmap = null;
                    }

                    if (_bitmap != null)
                        _grayBitmap = ImageUtil.ConvertToGrayscale(_bitmap);

                    Invalidate();
                }
            }
        }

        public event EventHandler Click;

        protected virtual void OnClick(EventArgs e)
        {
            var handler = Click;
            if (handler != null)
                handler(this, e);
        }

        public event EventHandler IsCheckedChanged;

        protected virtual void OnIsCheckedChanged(EventArgs e)
        {
            var handler = IsCheckedChanged;
            if (handler != null)
                handler(this, e);
        }

        public GdiButton()
        {
            Background = Brush.Transparent;
            IsEnabled = true;
            Padding = new Thickness(2);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            e.PreventBubble();

            if (!IsEnabled)
                return;

            Capture = true;

            _state |= State.Down;

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!IsEnabled)
                return;

            _state |= State.Over;

            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!IsEnabled)
                return;

            _state &= ~State.Over;

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!IsEnabled)
                return;

            e.PreventBubble();

            Capture = false;

            _state &= ~State.Down;

            if (IsMouseDirectlyOver)
                PerformClick();
        }

        public void PerformClick()
        {
            if (IsToggle)
            {
                IsChecked = !IsChecked;
            }

            OnClick(EventArgs.Empty);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            _state |= State.Focused;

            Invalidate();

            ElementUtil.FindHost(this).KeyUp += GdiButton_KeyUp;
        }

        void GdiButton_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Space:
                    if (IsEnabled)
                        PerformClick();
                    break;
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _state &= ~State.Focused;

            Invalidate();

            ElementUtil.FindHost(this).KeyUp -= GdiButton_KeyUp;
        }

        protected override Size MeasureOverride(Size desiredSize)
        {
            return new Size(
                (_bitmap == null ? 0 : _bitmap.Width) + Padding.Horizontal + 2,
                (_bitmap == null ? 0 : _bitmap.Height) + Padding.Vertical + 2
            );
        }

        protected override void OnPaint(ElementPaintEventArgs e)
        {
            var bounds = (Rectangle)e.Bounds;

            if (bounds.Width == 0 || bounds.Height == 0)
                return;

            bool enabled = (_state & State.Enabled) != 0;
            bool toggled = enabled && (_state & State.Checked) != 0;
            bool down = enabled && (_state & State.Down) != 0;
            bool over = enabled && !down && (IsMouseDirectlyOver || (_state & State.Focused) != 0);

            System.Drawing.Color startColor;
            System.Drawing.Color endColor;
            System.Drawing.Color? borderColor = null;

            if (down)
            {
                startColor = ColorTable.ButtonPressedGradientBegin;
                endColor = ColorTable.ButtonPressedGradientEnd;
                borderColor = ColorTable.ButtonPressedBorder;
            }
            else if (over)
            {
                startColor = ColorTable.ButtonSelectedGradientBegin;
                endColor = ColorTable.ButtonSelectedGradientEnd;
                borderColor = ColorTable.ButtonPressedBorder;
            }
            else if (toggled)
            {
                startColor = ColorTable.ButtonCheckedGradientBegin;
                endColor = ColorTable.ButtonCheckedGradientEnd;
                borderColor = ColorTable.ButtonCheckedHighlightBorder;
            }
            else
            {
                startColor = ColorTable.ToolStripGradientBegin;
                endColor = ColorTable.ToolStripGradientEnd;
            }

            using (var brush = new LinearGradientBrush(
                bounds,
                startColor,
                endColor,
                LinearGradientMode.Horizontal
            ))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }

            if (borderColor.HasValue)
            {
                using (var pen = new Pen(borderColor.Value))
                {
                    e.Graphics.DrawRectangle(
                        pen,
                        bounds.Left,
                        bounds.Top,
                        bounds.Width - 1,
                        bounds.Height - 1
                    );
                }
            }

            var bitmap = enabled ? _bitmap : _grayBitmap;

            if (bitmap != null)
            {
                e.Graphics.DrawImage(
                    bitmap,
                    bounds.X + (bounds.Width - bitmap.Width) / 2,
                    bounds.Y + (bounds.Height - bitmap.Height) / 2
                );
            }
        }

        [Flags]
        private enum State
        {
            None = 0,
            Over = 1 << 0,
            Down = 1 << 1,
            Focused = 1 << 2,
            Enabled = 1 << 3,
            Checked = 1 << 4
        }
    }
}
