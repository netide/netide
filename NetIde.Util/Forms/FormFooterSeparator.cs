using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class FormFooterSeparator : Control
    {
        [Category("Layout")]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue(DockStyle.Fill)]
        public new DockStyle Dock
        {
            get { return base.Dock; }
            set { base.Dock = value; }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(0, 1, 0, 1); }
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);

            Width = 0;
        }

        public FormFooterSeparator()
        {
            Dock = DockStyle.Fill;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            width = 2 + Padding.Horizontal;

            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var bounds = ClientRectangle;
            var padding = Padding;

            bounds = new Rectangle(
                bounds.X + padding.Left,
                bounds.Y + padding.Top,
                bounds.Width - padding.Horizontal,
                bounds.Height - padding.Vertical
            );

            ControlPaint.DrawBorder3D(
                e.Graphics,
                bounds,
                Border3DStyle.SunkenOuter
            );
        }
    }
}
