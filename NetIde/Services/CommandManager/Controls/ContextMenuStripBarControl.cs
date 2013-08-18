using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class ContextMenuStripBarControl : ContextMenuStrip, IBarControl
    {
        public Image Image { get; set; }

        event EventHandler IBarControl.DropDownOpening
        {
            add { }
            remove { }
        }

        public ControlControl CreateButton(IServiceProvider serviceProvider, NiCommandBarButton button)
        {
            return new ButtonControl.MenuItem(serviceProvider, button);
        }

        public ControlControl CreateComboBox(IServiceProvider serviceProvider, NiCommandBarComboBox comboBox)
        {
            throw new NetIdeException(Labels.ComboBoxNotSupported);
        }

        public ControlControl CreatePopup(IServiceProvider serviceProvider, NiCommandBarPopup popup)
        {
            return new PopupControl<MenuItemBarControl>(serviceProvider, popup, ToolStripItemDisplayStyle.ImageAndText);
        }
    }
}
