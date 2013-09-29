using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Setup.Pages
{
    public partial class ConfirmPage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public ConfirmPage()
        {
            InitializeComponent();
        }

        private void _previousButton_Click(object sender, EventArgs e)
        {
            MainForm.SetStep(-1);
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            MainForm.SetStep(1);
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            MainForm.Close();
        }

        private void ConfirmPage_Load(object sender, EventArgs e)
        {
            switch (SetupConfiguration.Mode)
            {
                case SetupMode.Install:
                    _formHeader.SubText = Labels.InstallComponentsConfirm;
                    _message.Text = Labels.InstallComponentsConfirmSubText;
                    break;

                case SetupMode.Update:
                    _formHeader.SubText = Labels.UpdateComponentsConfirm;
                    _message.Text = Labels.UpdateComponentsConfirmSubText;
                    break;
            }

            _formHeader.SubText = String.Format(_formHeader.SubText, Program.Configuration.Title);

            foreach (var package in SetupConfiguration.Packages)
            {
                if (
                    !package.Metadata.State.HasFlag(PackageState.SystemPackage) &&
                    package.InstalledVersion != package.Metadata.Version
                )
                    _packages.Items.Add(package.Metadata.Title);
            }
        }
    }
}
