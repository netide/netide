using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public static class ToolStripUtil
    {
        public static void FixSeparators(ToolStrip contextMenuStrip)
        {
            ToolStripSeparator lastSeparator = null;

            foreach (ToolStripItem item in contextMenuStrip.Items)
            {
                var separator = item as ToolStripSeparator;
                if (separator != null)
                {
                    separator.Visible = false;
                    lastSeparator = separator;
                }
                else if (item.Available && lastSeparator != null)
                {
                    lastSeparator.Visible = true;
                    lastSeparator = null;
                }
            }

            for (int i = contextMenuStrip.Items.Count - 1; i >= 0; i--)
            {
                var separator = contextMenuStrip.Items[i] as ToolStripSeparator;
                if (separator == null)
                    break;

                separator.Visible = false;
            }
        }
    }
}
