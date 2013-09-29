using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Update;

namespace NetIde.Setup.Pages
{
    public partial class FinishedPage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public FinishedPage()
        {
            InitializeComponent();
        }

        private void FinishedPage_Load(object sender, EventArgs e)
        {
            switch (SetupConfiguration.Mode)
            {
                case SetupMode.Install:
                    _headerLabel.Text = Labels.InstallComplete;
                    _wizardLabel.Text = Labels.InstallCompleteSubText;
                    break;

                case SetupMode.Update:
                    _headerLabel.Text = Labels.UpdateComplete;
                    _wizardLabel.Text = Labels.UpdateCompleteSubText;
                    break;
            }

            _headerLabel.Text = String.Format(_headerLabel.Text, Program.Configuration.Title);
            _wizardLabel.Text = String.Format(_wizardLabel.Text, Program.Configuration.Title);

            _start.Text = String.Format(_start.Text, Program.Configuration.Title);
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            if (_start.Checked)
            {
                string installationPath;

                using (var key = PackageRegistry.OpenRegistryRoot(false, Program.Configuration.Context))
                {
                    installationPath = (string)key.GetValue("InstallationPath");
                }

                string executable = PackageManager.GetEntryAssemblyLocation(installationPath);

                if (File.Exists(executable))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = executable,
                        WorkingDirectory = installationPath
                    });
                }
            }

            MainForm.SetStep(1);
        }
    }
}
