using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.CommandManager.Controls;

namespace NetIde.Support
{
    internal static class ToolStripUtil
    {
        public static void UpdateSeparatorVisibility(this ToolStripItemCollection items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            bool hadAnyVisible = false;

            for (int separatorIndex = 0; separatorIndex < items.Count; separatorIndex++)
            {
                var separator = items[separatorIndex] as ToolStripSeparator;
                if (separator == null)
                {
                    if (IsItemVisible(items[separatorIndex]))
                        hadAnyVisible = true;
                    continue;
                }

                bool hadVisible = false;

                for (int i = separatorIndex + 1; i < items.Count; i++)
                {
                    if (items[i] is ToolStripSeparator)
                        break;

                    if (IsItemVisible(items[i]))
                    {
                        hadVisible = true;
                        break;
                    }
                }

                bool forceInvisible = !hadAnyVisible;

                for (int i = separatorIndex - 1; i >= 0; i--)
                {
                    if (items[i] is ToolStripSeparator)
                        break;

                    if (IsItemVisible(items[i]))
                    {
                        forceInvisible = items[i].Alignment != separator.Alignment;
                        break;
                    }
                }

                separator.Visible = hadVisible && !forceInvisible;
            }
        }

        private static bool IsItemVisible(ToolStripItem item)
        {
            return ((ControlControl)item.Tag).NiCommand.IsVisible;
        }
    }
}
