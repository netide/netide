using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetIde.VisualStudio.Wizard
{
    [WizardStep(WizardStep.Welcome)]
    internal partial class WelcomeStep : WizardStepControl
    {
        public WelcomeStep()
        {
            InitializeComponent();
        }
    }
}
