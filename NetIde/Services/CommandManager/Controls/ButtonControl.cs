using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager.Controls
{
    internal static class ButtonControl
    {
        public class MenuItem : ButtonControl<ToolStripMenuItem>
        {
            public MenuItem(IServiceProvider serviceProvider, NiCommandBarButton control)
                : base(serviceProvider, control, ToolStripItemDisplayStyle.ImageAndText)
            {
            }

            protected override void UpdateItem()
            {
                base.UpdateItem();

                Item.ShortcutKeys = NiCommand.ShortcutKeys;
                Item.Checked = NiCommand.IsLatched;
            }
        }

        public class Button : ButtonControl<ToolStripButton>
        {
            public Button(IServiceProvider serviceProvider, NiCommandBarButton control)
                : base(serviceProvider, control, ToolStripItemDisplayStyle.Image)
            {
            }

            protected override void UpdateItem()
            {
                base.UpdateItem();

                Item.Checked = NiCommand.IsLatched;

                string toolTip = Item.ToolTipText;

                if (String.IsNullOrEmpty(toolTip))
                    toolTip = Item.Text;

                var shortcutKeys = NiCommand.ShortcutKeys;
                if (shortcutKeys != 0)
                {
                    if (toolTip.Length > 0)
                        toolTip += " ";
                    toolTip += "(" + ShortcutKeysUtil.ToDisplayString(shortcutKeys) + ")";
                }

                Item.ToolTipText = toolTip;
            }
        }
    }

    internal abstract class ButtonControl<T> : ControlControl<T>
        where T : ToolStripItem, new()
    {
        private readonly IServiceProvider _serviceProvider;

        public new NiCommandBarButton NiCommand
        {
            get { return (NiCommandBarButton)base.NiCommand; }
        }

        protected ButtonControl(IServiceProvider serviceProvider, NiCommandBarButton control, ToolStripItemDisplayStyle defaultDisplayStyle)
            : base(serviceProvider, control, defaultDisplayStyle)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;

            Item.Click += Item_Click;
        }

        void Item_Click(object sender, EventArgs e)
        {
            var commandManager = (INiCommandManager)_serviceProvider.GetService(typeof(INiCommandManager));

            object result;
            ErrorUtil.ThrowOnFailure(commandManager.Exec(NiCommand.Id, null, out result));
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
    }
}
