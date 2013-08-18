using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Core.Support
{
    internal class ToolStripSimpleRenderer : ToolStripProfessionalRenderer
    {
        public static readonly ToolStripSimpleRenderer Instance = new ToolStripSimpleRenderer();

        private ToolStripSimpleRenderer()
        {
        }

        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);

            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.CanOverflow = false;
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
                base.OnRenderToolStripBackground(e);
            else
                e.Graphics.FillRectangle(SystemBrushes.Control, e.AffectedBounds);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
                base.OnRenderToolStripBorder(e);
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);

            item.Margin = new Padding(0, 0, 0, 1);
        }
    }
}
