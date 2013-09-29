using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NetIde.Setup.Pages
{
    public partial class LicensePage : PageControl
    {
        public override IButtonControl AcceptButton
        {
            get { return _acceptButton; }
        }

        public override IButtonControl CancelButton
        {
            get { return _cancelButton; }
        }

        public LicensePage()
        {
            InitializeComponent();

            _formHeader.SubText = String.Format(_formHeader.SubText, Program.Configuration.Title);
            _agree.Text = String.Format(_agree.Text, Program.Configuration.Title);

            string license = File.ReadAllText(Program.Configuration.License);

            _license.Text = Regex.Replace(license, "\r?\n", "\r\n");
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
    }
}
