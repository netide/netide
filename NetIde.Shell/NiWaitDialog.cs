using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiWaitDialog : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed;
        private string _message;
        private string _progressText;
        private string _statusBarText;
        private float _progress;
        private INiWaitDialog _waitDialog;
        private bool _hasCancelled;

        public string Caption { get; set; }
        public TimeSpan ShowDelay { get; set; }
        public bool CanCancel { get; set; }
        public bool ShowRealProgress { get; set; }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    UpdateProgress();
                }
            }
        }

        public string ProgressText
        {
            get { return _progressText; }
            set
            {
                if (_progressText != value)
                {
                    _progressText = value;
                    UpdateProgress();
                }
            }
        }

        public string StatusBarText
        {
            get { return _statusBarText; }
            set
            {
                if (_statusBarText != value)
                {
                    _statusBarText = value;
                    UpdateProgress();
                }
            }
        }

        public float Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    UpdateProgress();
                }
            }
        }

        public bool HasCancelled
        {
            get { return _hasCancelled; }
        }

        public NiWaitDialog(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;

            ShowDelay = TimeSpan.FromSeconds(2);
        }

        public void Wait(Action<NiWaitDialog> callback)
        {
            if (callback == null)
                throw new ArgumentNullException("callback");

            using (var @event = new ManualResetEvent(false))
            {
                Exception exception = null;

                ThreadPool.QueueUserWorkItem(p => exception = DoCallback(callback, @event));

                Wait(new WaitHandle[] { @event });

                if (exception != null)
                    throw exception;
            }
        }

        private Exception DoCallback(Action<NiWaitDialog> callback, ManualResetEvent @event)
        {
            try
            {
                callback(this);

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
            finally
            {
                @event.Set();
            }
        }

        public void Wait(WaitHandle[] waitHandles)
        {
            if (waitHandles == null)
                throw new ArgumentNullException("waitHandles");
            if (waitHandles.Length == 0 || waitHandles.Any(p => p == null))
                throw new ArgumentException("Wait handles cannot be of zero length and cannot contain null entries");

            var waitHandlePointers = waitHandles.Select(p => p.SafeWaitHandle.DangerousGetHandle()).ToArray();

            if (_waitDialog == null)
            {
                var factory = (INiWaitDialogFactory)_serviceProvider.GetService(typeof(INiWaitDialogFactory));
                ErrorUtil.ThrowOnFailure(factory.CreateInstance(out _waitDialog));
            }

            ErrorUtil.ThrowOnFailure(_waitDialog.ShowWaitDialog(
                Caption,
                Message,
                ProgressText,
                StatusBarText,
                ShowDelay,
                CanCancel,
                ShowRealProgress,
                Progress,
                waitHandlePointers
            ));

            ErrorUtil.ThrowOnFailure(_waitDialog.HasCanceled(out _hasCancelled));
        }

        private void UpdateProgress()
        {
            if (_waitDialog != null)
            {
                ErrorUtil.ThrowOnFailure(_waitDialog.UpdateProgress(
                    Message,
                    ProgressText,
                    StatusBarText,
                    Progress,
                    !CanCancel,
                    out _hasCancelled
                ));
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_waitDialog != null)
                {
                    _waitDialog.Dispose();
                    _waitDialog = null;
                }

                _disposed = true;
            }
        }
    }
}
