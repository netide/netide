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
    [WizardStep(WizardStep.Signing)]
    internal partial class SigningStep : WizardStepControl
    {
        public SigningStep()
        {
            InitializeComponent();
            UpdateEnabled();
        }

        private void SigningStep_Load(object sender, EventArgs e)
        {
            _generateNew.Checked = Configuration.GenerateKeyFile;
            _keyFile.Path = Configuration.KeyFile;
        }

        private void _useExisting_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.GenerateKeyFile = !_useExisting.Checked;
            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _keyFileContainer.Enabled = _useExisting.Checked;
        }

        private void _keyFile_PathChanged(object sender, EventArgs e)
        {
            Configuration.KeyFile = _keyFile.Path;
        }

        public override bool CanNext()
        {
            return
                Configuration.GenerateKeyFile ||
                Configuration.KeyFile != null;
        }
    }
}
