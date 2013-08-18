using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NetIde.Win32;

namespace NetIde.Services.JobManager
{
    public partial class JobProgressControl : NetIde.Util.Forms.UserControl
    {
        public JobProgressControl()
        {
            InitializeComponent();
        }

        internal NiJob Job { get; set; }

        public void UpdateProgress()
        {
            _titleLabel.Text = (Job.Handler.Title ?? "").Replace("&", "&&");
            _statusLabel.Text = (Job.CurrentStatus ?? "").Replace("&", "&&");

            if (Job.Running)
            {
                if (Job.Progress.HasValue)
                {
                    _progressBar.Style = ProgressBarStyle.Continuous;
                    _progressBar.Value = (int)(Job.Progress * _progressBar.Maximum);
                }
                else
                {
                    _progressBar.Style = ProgressBarStyle.Marquee;
                }
            }
            else
            {
                _progressBar.Style = ProgressBarStyle.Continuous;
            }

            if (Job.Cancelled || (Job.Completed && !Job.Success))
                NativeMethods.SetProgressBarState(_progressBar, NativeMethods.PBST_ERROR);
        }

        private void BackgroundJobProgressControl_Load(object sender, EventArgs e)
        {
            UpdateProgress();
        }
    }
}
