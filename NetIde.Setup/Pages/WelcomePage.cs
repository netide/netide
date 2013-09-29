using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Setup.Pages
{
    public partial class WelcomePage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public WelcomePage()
        {
            InitializeComponent();
        }

        private void WelcomePage_Load(object sender, EventArgs e)
        {
            switch (SetupConfiguration.Mode)
            {
                case SetupMode.Install:
                    _headerLabel.Text = Labels.InstallWelcome;
                    _wizardLabel.Text = Labels.InstallWelcomeSubText;
                    break;

                case SetupMode.Update:
                    _headerLabel.Text = Labels.UpdateWelcome;
                    _wizardLabel.Text = Labels.UpdateWelcomeSubText;
                    break;
            }

            _headerLabel.Text = String.Format(_headerLabel.Text, Program.Configuration.Title);
            _wizardLabel.Text = String.Format(_wizardLabel.Text, Program.Configuration.Title);
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            MainForm.SetStep(1);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            MainForm.Close();
        }
    }
}
