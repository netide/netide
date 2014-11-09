using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.WaitDialog
{
    internal class NiWaitDialog : ServiceObject, INiWaitDialog
    {
        private const int WaitBeforeWaitCursor = 200;

        private readonly IServiceProvider _serviceProvider;
        private readonly SynchronizationContext _synchronizationContext;
        private WaitDialogForm _form;
        private bool _cancelled;
        private bool _showing;
        private INiStatusBar _statusBar;

        public NiWaitDialog(IServiceProvider serviceProvider, SynchronizationContext synchronizationContext)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;
            _synchronizationContext = synchronizationContext;
        }

        public HResult ShowWaitDialog(string caption, string message, string progressText, string statusBarText, TimeSpan showDelay, bool canCancel, bool realProgress, float progress, IntPtr[] waitHandles)
        {
            try
            {
                if (waitHandles == null)
                    throw new ArgumentNullException("waitHandles");
                if (waitHandles.Length == 0)
                    throw new ArgumentException("Wait handles cannot be of zero length");
                if (_showing)
                    throw new InvalidOperationException("Wait dialog is already showing");

                _showing = true;

                try 
                {
                    if (statusBarText != null)
                        SetStatusBarText(statusBarText);

                    int delay = (int)showDelay.TotalMilliseconds;

                    if (WaitForHandles(waitHandles, ref delay, WaitBeforeWaitCursor))
                        return HResult.OK;

                    Application.UseWaitCursor = true;
                    Cursor.Current = Cursors.WaitCursor;
                    Application.DoEvents();

                    try
                    {
                        if (WaitForHandles(waitHandles, ref delay, delay))
                            return HResult.OK;

                        _form = new WaitDialogForm(_synchronizationContext, caption, message, progressText, canCancel, realProgress, progress, waitHandles);

                        try
                        {
                            _cancelled = _form.ShowDialog(_serviceProvider) != DialogResult.OK;
                        }
                        finally
                        {
                            _form.Dispose();
                            _form = null;
                        }
                    }
                    finally
                    {
                        Application.UseWaitCursor = false;
                        Cursor.Current = Cursors.Default;

                        SetStatusBarText(null);
                    }
                }
                finally
                {
                    _showing = false;
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private bool WaitForHandles(IntPtr[] waitHandles, ref int delay, int wait)
        {
            if (delay <= 0)
                return false;

            int result = _synchronizationContext.Wait(waitHandles, false, Math.Min(delay, wait));

            if (result >= 0 && result < waitHandles.Length)
                return true;

            delay -= wait;

            return false;
        }

        private void SetStatusBarText(string statusBarText)
        {
            if (_statusBar == null)
            {
                if (statusBarText == null)
                    return;

                _statusBar = (INiStatusBar)_serviceProvider.GetService(typeof(INiStatusBar));
            }

            if (statusBarText == null)
                ErrorUtil.ThrowOnFailure(_statusBar.Clear());
            else
                ErrorUtil.ThrowOnFailure(_statusBar.SetText(statusBarText));
        }

        public HResult UpdateProgress(string message, string progressText, string statusBarText, float progress, bool disableCancel, out bool cancelled)
        {
            cancelled = false;

            try
            {
                var form = _form;
                if (form != null)
                    form.UpdateProgress(message, progressText, progress, disableCancel);

                SetStatusBarText(statusBarText);

                cancelled = _cancelled;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult HasCanceled(out bool cancelled)
        {
            cancelled = _cancelled;

            return HResult.OK;
        }
    }
}
