using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Services.CommandManager.Controls
{
    internal abstract class ControlControl : IDisposable
    {
        private bool _disposed;

        public NiCommandBarControl NiCommand { get; private set; }
        public ToolStripItem Item { get; private set; }

        protected ToolStripItemDisplayStyle DefaultDisplayStyle { get; private set; }

        protected ControlControl(IServiceProvider serviceProvider, NiCommandBarControl control, ToolStripItem item, ToolStripItemDisplayStyle defaultDisplayStyle)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (control == null)
                throw new ArgumentNullException("control");
            if (item == null)
                throw new ArgumentNullException("item");

            NiCommand = control;

            Item = item;
            DefaultDisplayStyle = defaultDisplayStyle;
            Item.Tag = this;

            UpdateItem();

            NiCommand.AppearanceChanged += Control_AppearanceChanged;
        }

        void Control_AppearanceChanged(object sender, EventArgs e)
        {
            UpdateItem();
        }

        protected virtual void UpdateItem()
        {
            Item.Visible = NiCommand.IsVisible;
            Item.Enabled = NiCommand.IsEnabled;
            Item.ToolTipText = NiCommand.ToolTip;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (NiCommand != null)
                {
                    NiCommand.AppearanceChanged -= Control_AppearanceChanged;
                    NiCommand = null;
                }

                if (Item != null)
                {
                    Item.Dispose();
                    Item = null;
                }

                _disposed = true;
            }
        }
    }

    internal abstract class ControlControl<T> : ControlControl
        where T : ToolStripItem, new()
    {
        public new T Item
        {
            get { return (T)base.Item; }
        }

        protected ControlControl(IServiceProvider serviceProvider, NiCommandBarControl control, ToolStripItemDisplayStyle defaultDisplayStyle)
            : base(serviceProvider, control, new T(), defaultDisplayStyle)
        {
        }
    }
}
