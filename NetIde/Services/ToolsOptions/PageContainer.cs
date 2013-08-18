using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Services.ToolsOptions
{
    internal class PageContainer : Panel
    {
        protected override Padding DefaultPadding
        {
            get { return new Padding(0, 0, 0, 6); }
        }

        public PageContainer()
        {
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);

            Padding = DefaultPadding;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawLine(
                SystemPens.ControlDark,
                0, Height - 2, Width, Height - 2
            );
            e.Graphics.DrawLine(
                SystemPens.ControlLightLight,
                0, Height - 1, Width, Height - 1
            );
        }
    }
}
