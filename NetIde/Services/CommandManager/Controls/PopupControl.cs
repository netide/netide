using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.Env;
using NetIde.Services.MenuManager;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Services.CommandManager.Controls
{
    internal class PopupControl<T> : ControlControl<T>
        where T : ToolStripItem, IBarControl, new()
    {
        private readonly IServiceProvider _serviceProvider;
        private GroupManager _groupManager;
        private bool _disposed;
        private readonly NiMenuManager _menuManager;
        
        public new NiCommandBarPopup NiCommand
        {
            get { return (NiCommandBarPopup)base.NiCommand; }
        }

        public PopupControl(IServiceProvider serviceProvider, NiCommandBarPopup control, ToolStripItemDisplayStyle defaultDisplayStyle)
            : base(control, defaultDisplayStyle)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;
            _groupManager = new GroupManager(NiCommand, serviceProvider, Item);
            _menuManager = (NiMenuManager)serviceProvider.GetService(typeof(INiMenuManager));

            Item.DropDownOpening += Item_DropDownOpening;
        }

        void Item_DropDownOpening(object sender, EventArgs e)
        {
            _menuManager.QueryStatus(NiCommand);
        }

        protected override void UpdateItem()
        {
            base.UpdateItem();

            Item.Text = NiCommand.Text;

            if (NiCommand.DisplayStyle == NiCommandDisplayStyle.Default)
                Item.DisplayStyle = DefaultDisplayStyle;
            else
                Item.DisplayStyle = Enum<ToolStripItemDisplayStyle>.Parse(NiCommand.DisplayStyle.ToString());

            if (NiCommand.Image != null)
            {
                var env = (NiEnv)_serviceProvider.GetService(typeof(INiEnv));
                Item.Image = env.ResourceManager.GetImage(NiCommand.Image);
            }
            else
            {
                Item.Image = NiCommand.Bitmap;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (_groupManager != null)
                {
                    _groupManager.Dispose();
                    _groupManager = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
