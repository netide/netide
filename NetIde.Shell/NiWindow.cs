using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiWindow : ServiceObject, IWin32Window, INiPreMessageFilter
    {
        private bool _hwndCreated;
        private IntPtr _hwnd;
        private bool _disposed;

        public IWin32Window Window { get; set; }

        IntPtr IWin32Window.Handle
        {
            get
            {
                if (!_hwndCreated)
                    CreateHandle();

                return _hwnd;
            }
        }

        private void CreateHandle()
        {
            _hwndCreated = true;

            if (Window != null)
                _hwnd = Window.Handle;
        }

        public HResult PreFilterMessage(ref NiMessage message, out bool processed)
        {
            processed = false;

            try
            {
                if (Window == null)
                    return HResult.OK;

                var control = FindTarget(message.HWnd);

                if (control == null)
                    return HResult.OK;

                var msg = (Message)message;

                processed = control.PreProcessMessage(ref msg);

                message = msg;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private Control FindTarget(IntPtr hWnd)
        {
            // The messages we get here may not be messages of a control that
            // is in our AppDomain. Because of this, we find a control that is
            // in our AppDomain and request that control to handle it. If
            // that control is a NativeWindowHost, that itself will redirect
            // the message to the correct AppDomain.

            while (hWnd != IntPtr.Zero && hWnd != Window.Handle)
            {
                var control = Control.FromHandle(hWnd);

                if (control != null)
                    return control;

                hWnd = GetParent(hWnd);
            }

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                var disposable = Window as IDisposable;

                if (disposable != null)
                    disposable.Dispose();

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hwnd);
    }
}
