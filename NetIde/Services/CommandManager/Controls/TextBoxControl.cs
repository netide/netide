using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class TextBoxControl : ControlControl<ToolStripSpringTextBox>
    {
        private bool _suppressPublish;
        private readonly IServiceProvider _serviceProvider;
        private string _lastPublished;

        public new NiCommandBarTextBox NiCommand
        {
            get { return (NiCommandBarTextBox)base.NiCommand; }
        }

        public TextBoxControl(IServiceProvider serviceProvider, NiCommandBarControl control)
            : base(serviceProvider, control, ToolStripItemDisplayStyle.None)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;

            Item.Leave += (s, e) => Publish();
            Item.TextChanged += (s, e) => Publish();
        }

        private void Publish()
        {
            if (_suppressPublish)
                return;

            _suppressPublish = true;

            try
            {
                NiCommand.Value = Item.Text;

                if (NiCommand.Value != _lastPublished)
                {
                    _lastPublished = NiCommand.Value;

                    var commandManager = (INiCommandManager)_serviceProvider.GetService(typeof(INiCommandManager));

                    object unused;
                    ErrorUtil.ThrowOnFailure(commandManager.Exec(NiCommand.Id, Item.Text, out unused));
                }
            }
            finally
            {
                _suppressPublish = false;
            }
        }

        protected override void UpdateItem()
        {
            base.UpdateItem();

            Item.Spring = NiCommand.Style == NiCommandBarTextBoxStyle.Stretch;

            string text = NiCommand.Value ?? String.Empty;

            if (Item.Text != text)
            {
                _suppressPublish = true;

                try
                {
                    Item.Text = text;
                }
                finally
                {
                    _suppressPublish = false;
                }
            }
        }
    }
}
