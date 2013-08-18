using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace NetIde.Core.PackageManagement
{
    internal class PackagePageButton : Control
    {
        private const TextFormatFlags TextFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter;

        private ButtonState _state = ButtonState.Inactive;
        private System.Drawing.Image _image;

        private ButtonState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    Invalidate();
                }
            }
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;

                PerformLayout();
                Invalidate();
            }
        }

        public Image Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;

                    PerformLayout();
                    Invalidate();
                }
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(2, 1, 2, 1); }
        }

        [DefaultValue(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [RefreshProperties(RefreshProperties.All)]
        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        public PackagePageButton()
        {
            AutoSize = true;
            Padding = DefaultPadding;
            TabStop = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rect = ClientRectangle;

            if (State != ButtonState.Inactive)
            {
                if (Application.RenderWithVisualStyles)
                {
                    var style =
                        State == ButtonState.Normal
                        ? VisualStyleElement.Button.PushButton.Normal
                        : VisualStyleElement.Button.PushButton.Pressed;

                    var renderer = new VisualStyleRenderer(style);

                    renderer.DrawBackground(e.Graphics, rect);
                }
                else
                {
                    ControlPaint.DrawButton(
                        e.Graphics,
                        rect,
                        State
                    );
                }
            }

            var padding = Padding;

            if (!String.IsNullOrEmpty(Text))
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    new Rectangle(
                        rect.Left + padding.Left,
                        rect.Top + padding.Top,
                        rect.Width - padding.Horizontal,
                        rect.Height - padding.Vertical
                    ),
                    ForeColor,
                    Color.Transparent,
                    TextFlags
                );
            }

            if (_image != null)
            {
                var imageSize = _image.Size;

                var imageOffset = new Point(
                    (Width - imageSize.Width) / 2,
                    (Height - imageSize.Height) / 2
                );

                e.Graphics.DrawImage(
                    _image,
                    imageOffset
                );
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            UpdateState();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            UpdateState();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            UpdateState();

            if (State == ButtonState.Pushed)
                OnClick(EventArgs.Empty);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            bool isOver = ClientRectangle.Contains(PointToClient(MousePosition));
            bool isDown = (MouseButtons & MouseButtons.Left) != 0;

            State =
                isOver
                ? (isDown ? ButtonState.Pushed : ButtonState.Normal)
                : ButtonState.Inactive;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var textSize = TextRenderer.MeasureText(
                String.IsNullOrEmpty(Text)
                ? " "
                : Text,
                Font,
                new Size(int.MaxValue, int.MaxValue),
                TextFlags
            );

            var imageSize = _image != null ? _image.Size : Size.Empty;

            var size = new Size(
                Math.Max(textSize.Width, imageSize.Width),
                Math.Max(textSize.Height, imageSize.Height)
            );

            var padding = Padding;

            return new Size(size.Width + padding.Horizontal, size.Height + padding.Vertical);
        }
    }
}
