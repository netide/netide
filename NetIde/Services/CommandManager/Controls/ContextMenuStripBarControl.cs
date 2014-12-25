using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager.Controls
{
    internal class ContextMenuStripBarControl : ContextMenuStrip, IBarControl
    {
        public Image Image { get; set; }

        public event EventHandler QueryStatus;

        protected virtual void OnQueryStatus(EventArgs e)
        {
            var ev = QueryStatus;
            if (ev != null)
                ev(this, e);
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            OnQueryStatus(EventArgs.Empty);
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
