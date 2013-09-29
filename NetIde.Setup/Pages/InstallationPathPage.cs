using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Forms;

namespace NetIde.Setup.Pages
{
    public partial class InstallationPathPage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public InstallationPathPage()
        {
            InitializeComponent();

            _formHeader.SubText = String.Format(_formHeader.SubText, Program.Configuration.Title);
            _introduction.Text = String.Format(_introduction.Text, Program.Configuration.Title);
        }

        private void _browse_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowser();

            if (dialog.ShowDialog(this) == DialogResult.OK)
                _targetPath.Text = dialog.SelectedPath;
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

        private void InstallationPathPage_Load(object sender, EventArgs e)
        {
            _targetPath.Text = SetupConfiguration.InstallationPath;
        }

        private void _targetPath_TextChanged(object sender, EventArgs e)
        {
            SetupConfiguration.InstallationPath = _targetPath.Text;
        }
    }
}
