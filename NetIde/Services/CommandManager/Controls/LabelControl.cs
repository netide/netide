using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Services.CommandManager.Controls
{
    internal class LabelControl : ControlControl<ToolStripLabel>
    {
        public new NiCommandBarLabel NiCommand
        {
            get { return (NiCommandBarLabel)base.NiCommand; }
        }

        public LabelControl(IServiceProvider serviceProvider, NiCommandBarControl control)
            : base(serviceProvider, control, ToolStripItemDisplayStyle.None)
        {
        }

        protected override void UpdateItem()
        {
            base.UpdateItem();

            Item.Text = NiCommand.Text;
        }
    }
}
