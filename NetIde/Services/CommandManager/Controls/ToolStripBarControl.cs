using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class ToolStripBarControl : System.Windows.Forms.ToolStrip, IBarControl
    {
        Image IBarControl.Image { get; set; }

        event EventHandler IBarControl.DropDownOpening
        {
            add { }
            remove { }
        }

        public ControlControl CreateButton(IServiceProvider serviceProvider, NiCommandBarButton button)
        {
            return new ButtonControl.Button(serviceProvider, button);
        }

        public ControlControl CreateComboBox(IServiceProvider serviceProvider, NiCommandBarComboBox comboBox)
        {
            return new ComboBoxControl(serviceProvider, comboBox);
        }

        public ControlControl CreatePopup(IServiceProvider serviceProvider, NiCommandBarPopup popup)
        {
            return new PopupControl<DropDownButtonBarControl>(serviceProvider, popup, ToolStripItemDisplayStyle.Image);
        }
    }
}