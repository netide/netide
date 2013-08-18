using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using DockStyle = System.Windows.Forms.DockStyle;

namespace NetIde.Services.JobManager
{
    public partial class JobForm : NetIde.Util.Forms.Form
    {
        private int _jobsSeen;
        private int _jobsCompleted;
        private bool _detailsShowing = true;
        private int _detailsHeight;
        private readonly Dictionary<NiJob, JobProgressControl> _controls = new Dictionary<NiJob, JobProgressControl>();
        private NiJobManager _jobManager;

        public JobForm()
        {
            InitializeComponent();

            Disposed += JobForm_Disposed;

            AllowClose(false);
        }

        void _jobManager_JobAdded(object sender, JobEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<NiJob>(AddJob), e.Job);
            else
                AddJob(e.Job);
        }

        private void _jobManager_JobRemoved(object sender, JobEventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new Action<NiJob>(RemoveJob), e.Job);
            else
                RemoveJob(e.Job);
        }

        private void _jobManager_ProgressChanged(object sender, JobEventArgs e)
        {
            if (!IsDisposed)
            {
                if (InvokeRequired)
                    BeginInvoke(new Action<NiJob>(UpdateProgress), e.Job);
                else
                    UpdateProgress(e.Job);
            }
        }

        void JobForm_Disposed(object sender, System.EventArgs e)
        {
            _jobManager.JobAdded -= _jobManager_JobAdded;
            _jobManager.JobRemoved -= _jobManager_JobRemoved;
            _jobManager.ProgressChanged -= _jobManager_ProgressChanged;
        }

        private void JobForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = !_acceptButton.Enabled;
        }

        private void AllowClose(bool allow)
        {
            _acceptButton.Enabled = allow;

            CloseButtonEnabled = allow;
        }

        private void JobForm_Load(object sender, EventArgs e)
        {
            _jobManager = (NiJobManager)GetService(typeof(INiJobManager));

            _jobManager.JobAdded += _jobManager_JobAdded;
            _jobManager.JobRemoved += _jobManager_JobRemoved;
            _jobManager.ProgressChanged += _jobManager_ProgressChanged;

            ToggleDetails();

            foreach (var job in _jobManager.GetAllJobs())
            {
                AddJob(job);
            }

            if (_jobsSeen == 0)
                Dispose();
        }

        private void AddJob(NiJob job)
        {
            if (_controls.ContainsKey(job))
                return;

            _jobsSeen++;

            var control = new JobProgressControl
            {
                Job = job,
                Dock = DockStyle.Top
            };

            _controls.Add(job, control);

            _progressItemsPanel.Controls.Add(control);
            _progressItemsPanel.Controls.SetChildIndex(control, 0);

            if (job.Running)
                UpdateOwnProgress(job);
        }

        private void RemoveJob(NiJob job)
        {
            JobProgressControl control;

            if (_controls.TryGetValue(job, out control))
            {
                _jobsCompleted++;

                _progressItemsPanel.Controls.Remove(control);

                control.Dispose();

                _controls.Remove(job);

                if (_controls.Count == 0)
                    Dispose();
            }
        }

        private void UpdateProgress(NiJob job)
        {
            if (job != null)
            {
                JobProgressControl control;

                if (_controls.TryGetValue(job, out control))
                {
                    control.UpdateProgress();

                    if (job.Running)
                        UpdateOwnProgress(job);
                }
            }
        }

        private void UpdateOwnProgress(NiJob job)
        {
            if (_progressItemsPanel.Controls.Count > 0)
            {
                double step = (double)_progressBar.Maximum / _jobsSeen;

                Text = job.Handler.Title;

                _statusTextBox.Text = job.CurrentStatus;

                if (!job.Progress.HasValue && _jobsCompleted == 0)
                {
                    _progressBar.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    _progressBar.Style = ProgressBarStyle.Continuous;

                    _progressBar.Value = (int)(
                        _jobsCompleted * step +
                        job.Progress.GetValueOrDefault(0) * step
                    );
                }
            }
        }

        private void ToggleDetails()
        {
            _detailsShowing = !_detailsShowing;

            if (_detailsShowing)
            {
                _detailsButton.Text = Labels.LessDetails;

                Height += _detailsHeight;

                _progressItemsOuterPanel.Visible = true;
            }
            else
            {
                _detailsHeight = _progressItemsOuterPanel.Height + _progressItemsOuterPanel.Margin.Vertical;

                _detailsButton.Text = Labels.MoreDetails;
                _progressItemsOuterPanel.Visible = false;

                Height -= _detailsHeight;
            }
        }

        private void _detailsButton_Click(object sender, EventArgs e)
        {
            ToggleDetails();
        }
    }
}
