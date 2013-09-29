using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Setup.Installation;

namespace NetIde.Setup.Pages
{
    public partial class InstallingPage : PageControl
    {
        public override bool CanClose
        {
            get { return false; }
        }

        public InstallingPage()
        {
            InitializeComponent();
        }

        private void _showDetails_Click(object sender, EventArgs e)
        {
            _showDetails.Visible = false;
            _progressListBox.Visible = true;
        }

        private void InstallingPage_Load(object sender, EventArgs e)
        {
            switch (SetupConfiguration.Mode)
            {
                case SetupMode.Install:
                    _formHeader.Text = Labels.Installing;
                    _formHeader.SubText = Labels.InstallingSubText;
                    break;

                case SetupMode.Update:
                    _formHeader.Text = Labels.Updating;
                    _formHeader.SubText = Labels.UpdatingSubText;
                    break;
            }

            _formHeader.SubText = String.Format(_formHeader.SubText, Program.Configuration.Title);
            _progressListBox.Visible = false;

            new Thread(ThreadProc).Start();
        }

        private void ThreadProc()
        {
            try
            {
                using (var installer = new PackageInstaller(SetupConfiguration, new Progress(this)))
                {
                    installer.Install();
                }

                BeginInvoke(new Action(() => MainForm.SetStep(1)));
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show(
                        this,
                        String.Format(
                            Labels.UnexpectedSituation,
                            ex.Message
                        ),
                        Program.Configuration.Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    MainForm.Dispose();
                }));
            }
        }

        private void AddLog(string message)
        {
            _progressLabel.Text = message;
            _progressListBox.Items.Add(message);

            int visibleItems = _progressListBox.ClientSize.Height / _progressListBox.ItemHeight;

            _progressListBox.TopIndex = Math.Max(_progressListBox.Items.Count - visibleItems + 1, 0);
        }

        private void SetProgress(double progress)
        {
            _progressBar.Value = (int)(_progressBar.Maximum * progress);
        }

        private class Progress : IProgress
        {
            private readonly InstallingPage _page;

            public Progress(InstallingPage page)
            {
                _page = page;
            }

            public void AddLog(string message)
            {
                _page.BeginInvoke(new Action(() => _page.AddLog(message)));
            }

            public void SetProgress(double progress)
            {
                _page.BeginInvoke(new Action(() => _page.SetProgress(progress)));
            }
        }
    }
}
