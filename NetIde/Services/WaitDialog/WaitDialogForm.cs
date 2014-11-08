using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NetIde.Services.WaitDialog
{
    internal partial class WaitDialogForm : DialogForm
    {
        private readonly SynchronizationContext _synchronizationContext;

        public WaitDialogForm(SynchronizationContext synchronizationContext, string caption, string message, string progressText, bool canCancel, bool realProgress, float progress, IntPtr[] waitHandles)
        {
            InitializeComponent();

            _synchronizationContext = synchronizationContext;
            _caption.Text = caption ?? Labels.NetIde;
            _message.Text = message;
            _progressText.Text = progressText;

            if (!canCancel)
                _cancelButton.Dispose();

            if (realProgress)
            {
                _progressBar.Minimum = 0;
                _progressBar.Maximum = _progressBar.Width;
                _progressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar(progress);
            }
            else
            {
                _progressBar.Style = ProgressBarStyle.Marquee;
            }

            ThreadPool.QueueUserWorkItem(SignalWaitHandleCompletion, waitHandles);
        }

        protected override void SetVisibleCore(bool value)
        {
            if (value && Owner == null)
                StartPosition = FormStartPosition.CenterScreen;

            base.SetVisibleCore(value);
        }

        private void SignalWaitHandleCompletion(object state)
        {
            // We do not care whether this throws, because there really isn't a
            // fallback action to take.

            SEH.SinkExceptions(() => _synchronizationContext.Wait((IntPtr[])state, false, -1));

            SEH.SinkExceptions(() => BeginInvoke(new Action(Dispose)));
        }

        private void UpdateProgressBar(float progress)
        {
            _progressBar.Value = Math.Min(
                Math.Max((int)(_progressBar.Maximum * progress), _progressBar.Minimum),
                _progressBar.Maximum
            );
        }

        public void UpdateProgress(string message, string progressText, float progress, bool disableCancel)
        {
            _synchronizationContext.Post(
                p =>
                {
                    _message.Text = message;
                    _progressText.Text = progressText;
                    UpdateProgressBar(progress);
                    _cancelButton.Enabled = !disableCancel;
                },
                null
            );
        }
    }
}
