using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal static class IBarControlExtensions
    {
        public static void UpdateSeparatorVisibility(this IBarControl self)
        {
            var items = self.Items;
            bool previousVisible = false;

            for (int separatorIndex = 0; separatorIndex < items.Count; separatorIndex++)
            {
                var separator = items[separatorIndex] as ToolStripSeparator;
                if (separator == null)
                    continue;

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

                bool forceInvisible = false;

                for (int i = separatorIndex - 1; i >= 0; i--)
                {
                    if (IsItemVisible(items[i]))
                    {
                        forceInvisible = items[i].Alignment != separator.Alignment;
                        break;
                    }
                }

                separator.Visible = hadVisible && previousVisible && !forceInvisible;
                previousVisible = hadVisible;
            }
        }

        private static bool IsItemVisible(ToolStripItem item)
        {
            return ((ControlControl)item.Tag).NiCommand.IsVisible;
        }
    }
}
