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

                if (separator != null)
                {
                    bool hadVisible = false;

                    for (int i = separatorIndex + 1; i < items.Count; i++)
                    {
                        if (items[i] is ToolStripSeparator)
                            break;

                        if (((ControlControl)items[i].Tag).NiCommand.IsVisible)
                        {
                            hadVisible = true;
                            break;
                        }
                    }

                    separator.Visible = hadVisible && previousVisible;
                    previousVisible = hadVisible;
                }
            }
        }
    }
}
