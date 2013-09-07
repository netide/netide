using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetIde.VisualStudio.Wizard
{
    internal class BaseForm : NetIde.Util.Forms.Form
    {
        [Localizable(true)]
        [DefaultValue(FormStartPosition.CenterParent)]
        public new FormStartPosition StartPosition
        {
            get { return base.StartPosition; }
            set { base.StartPosition = value; }
        }

        [DefaultValue(false)]
        public new bool ShowInTaskbar
        {
            get { return base.ShowInTaskbar; }
            set { base.ShowInTaskbar = value; }
        }

        [DefaultValue(false)]
        public new bool ShowIcon
        {
            get { return base.ShowIcon; }
            set { base.ShowIcon = value; }
        }

        public BaseForm()
        {
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            ShowIcon = false;
        }
    }
}
