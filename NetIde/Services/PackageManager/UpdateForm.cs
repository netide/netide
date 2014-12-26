using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.PackageManager
{
    internal partial class UpdateForm : DialogForm
    {
        private readonly IList<PendingUpdate> _pendingUpdates;
        private INiShell _shell;

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _shell = (INiShell)GetService(typeof(INiShell));
            }
        }

        public UpdateForm(IList<PendingUpdate> pendingUpdates)
        {
            if (pendingUpdates == null)
                throw new ArgumentNullException("pendingUpdates");

            _pendingUpdates = pendingUpdates;

            InitializeComponent();
        }

        private void UpdateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                Site.CreateTaskDialog()
                    .MainIcon(NiTaskDialogIcon.Information)
                    .MainInstruction(Labels.CannotCancelOperation)
                    .Alert(this);
            }
        }

        private void UpdateForm_Shown(object sender, EventArgs e)
        {
            var updater = new PackageUpdater(this, _pendingUpdates);

            updater.ProgressChanged += (s, ea) => UpdateProgress(ea.Progress, ea.CurrentStep, ea.TotalSteps);
            updater.Completed += (s, ea) => CompleteUpdate(ea.Exception);

            updater.Start();
        }

        private void CompleteUpdate(Exception exception)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => CompleteUpdate(exception)));
                return;
            }

            if (exception != null)
            {
                Site.CreateTaskDialog()
                    .MainInstruction(String.Format(Labels.UpdateFailed, exception.Message))
                    .Alert(this);
            }

            Dispose();
        }

        private void UpdateProgress(string progress, int currentStep, int totalSteps)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateProgress(progress, currentStep, totalSteps)));
                return;
            }

            _progressLabel.Text = progress;

            if (_progressBar.Maximum != totalSteps)
                _progressBar.Maximum = totalSteps;
            if (_progressBar.Value != currentStep)
                _progressBar.Value = currentStep;
        }
    }
}
