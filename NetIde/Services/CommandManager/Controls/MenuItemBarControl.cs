using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class MenuItemBarControl : ToolStripMenuItem, IBarControl
    {
        ToolStripItemCollection IBarControl.Items
        {
            get { return DropDownItems; }
        }

        public event EventHandler QueryStatus;

        protected virtual void OnQueryStatus(EventArgs e)
        {
            var ev = QueryStatus;
            if (ev != null)
                ev(this, e);
        }

        public MenuItemBarControl()
        {
            DropDown.Opening += (s, e) => OnQueryStatus(EventArgs.Empty);
        }

        public ControlControl CreateButton(IServiceProvider serviceProvider, NiCommandBarButton button)
        {
            return new ButtonControl.MenuItem(serviceProvider, button);
        }

        public ControlControl CreateComboBox(IServiceProvider serviceProvider, NiCommandBarComboBox comboBox)
        {
            throw new NetIdeException(NeutralResources.ComboBoxNotSupported);
        }

        public ControlControl CreateTextBox(IServiceProvider serviceProvider, NiCommandBarTextBox textBox)
        {
            throw new NetIdeException(NeutralResources.TextBoxNotSupported);
        }

        public ControlControl CreatePopup(IServiceProvider serviceProvider, NiCommandBarPopup popup)
        {
            return new PopupControl<MenuItemBarControl>(serviceProvider, popup, ToolStripItemDisplayStyle.ImageAndText);
        }

        public ControlControl CreateLabel(IServiceProvider serviceProvider, NiCommandBarLabel label)
        {
            throw new NetIdeException(NeutralResources.LabelNotSupported);
        }
    }
}