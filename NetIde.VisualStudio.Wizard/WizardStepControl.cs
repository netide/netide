using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetIde.VisualStudio.Wizard
{
    internal class WizardStepControl : NetIde.Util.Forms.UserControl
    {
        public WizardConfiguration Configuration { get; set; }

        public virtual bool CanNext()
        {
            return true;
        }
    }
}
