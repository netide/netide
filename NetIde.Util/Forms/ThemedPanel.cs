using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace NetIde.Util.Forms
{
    public class ThemedPanel : Panel
    {
        private Color _borderColor;

        [Category("Appearance")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Window")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(1); }
        }

        public ThemedPanel()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);

            BackColor = SystemColors.Window;
            UpdateThemeData();
        }

        private void UpdateThemeData()
        {
            if (!ControlUtil.GetIsInDesignMode(this) && Application.RenderWithVisualStyles)
            {
                _borderColor = new VisualStyleRenderer("ListView", 0, 0).GetColor(ColorProperty.BorderColor);

                if (_borderColor == Color.Black)
                    _borderColor = SystemColors.ControlDark;
            }
            else
            {
                _borderColor = SystemColors.ControlDark;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var bounds = ClientRectangle;

            bounds.Width--;
            bounds.Height--;

            using (var pen = new Pen(_borderColor))
            {
                e.Graphics.DrawRectangle(pen, bounds);
            }

            bounds = ClientRectangle;

            bounds.Inflate(-1, -1);

            using (var brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(brush, bounds);
            }
        }
    }
}
