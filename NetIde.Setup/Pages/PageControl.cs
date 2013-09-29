using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Setup.Pages
{
    public class PageControl : NetIde.Util.Forms.UserControl
    {
        public virtual IButtonControl AcceptButton
        {
            get { return null; }
        }

        public virtual IButtonControl CancelButton
        {
            get { return null; }
        }

        public virtual bool CanClose
        {
            get { return true; }
        }

        public MainForm MainForm
        {
            get { return (MainForm)FindForm(); }
        }

        public SetupConfiguration SetupConfiguration
        {
            get { return MainForm.SetupConfiguration; }
        }
    }
}
