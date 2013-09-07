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
    [WizardStep(WizardStep.Context)]
    internal partial class ContextStep : WizardStepControl
    {
        public ContextStep()
        {
            InitializeComponent();
        }

        private void ContextStep_Load(object sender, EventArgs e)
        {
            _packageContext.Text = Configuration.ReplacementsDictionary.GetValueOrDefault(ReplacementVariables.PackageContext, Configuration.ReplacementsDictionary["$projectname$"]);
        }

        private void _packageContext_TextChanged(object sender, EventArgs e)
        {
            Configuration.ReplacementsDictionary[ReplacementVariables.PackageContext] = _packageContext.Text;
        }

        public override bool CanNext()
        {
            return
                !String.IsNullOrEmpty(Configuration.ReplacementsDictionary[ReplacementVariables.PackageContext]);
        }
    }
}
