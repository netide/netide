using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiIsolationClient : UserControl, INiIsolationClient
    {
        private INiIsolationHost _host;
        private int _select;

        public NiIsolationClient()
        {
            SetStyle(ControlStyles.Selectable, true);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public HResult SetHost(INiIsolationHost host)
        {
            try
            {
                if (host == null)
                    throw new ArgumentNullException("host");

                _host = host;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Select(bool directed, bool forward)
        {
            if (_select > 0)
                return;

            _select++;

            try
            {
                ErrorUtil.ThrowOnFailure(_host.SelectNextControl(!directed || forward));
            }
            finally
            {
                _select--;
            }
        }

        private Control FindTarget(IntPtr hWnd)
        {
            // The messages we get here may not be messages of a control that
            // is in our AppDomain. Because of this, we find a control that is
            // in our AppDomain and request that control to handle it. If
            // that control is a NativeWindowHost, that itself will redirect
            // the message to the correct AppDomain.

            while (hWnd != IntPtr.Zero && hWnd != Handle)
            {
                var control = FromHandle(hWnd);

                if (control != null)
                    return control;

                hWnd = NativeMethods.GetParent(hWnd);
            }

            return null;
        }

        HResult INiIsolationClient.PreviewKeyDown(Keys keyData)
        {
            try
            {
                var result = HResult.False;

                var target = FindTarget(NativeMethods.GetFocus());
                if (target != null)
                {
                    var e = new PreviewKeyDownEventArgs(keyData);

                    ControlStubs.ControlOnPreviewKeyDown(target, e);

                    result = e.IsInputKey ? HResult.OK : HResult.False;
                }

                return result;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationClient.PreProcessMessage(ref NiMessage message, out PreProcessMessageResult preProcessMessageResult)
        {
            preProcessMessageResult = 0;

            try 
            {
                var result = HResult.False;

                var target = FindTarget(message.HWnd);
                if (target != null)
                {
                    Message msg = message;
                    result = target.PreProcessMessage(ref msg) ? HResult.OK : HResult.False;
                    message = msg;

                    if (ControlStubs.ControlGetState2(target, ControlStubs.STATE2_INPUTKEY))
                        preProcessMessageResult |= PreProcessMessageResult.IsInputKey;
                    if (ControlStubs.ControlGetState2(target, ControlStubs.STATE2_INPUTCHAR))
                        preProcessMessageResult |= PreProcessMessageResult.IsInputChar;
                }

                return result;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationClient.ProcessMnemonic(char charCode)
        {
            try
            {
                return ProcessMnemonic(charCode) ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationClient.SelectNextControl(bool forward)
        {
            try
            {
                if (_select > 0)
                    return HResult.False;

                _select++;

                try
                {
                    // The host was the target of the next control selection.
                    // The call is forwarded here and we select our next control.
                    // Wrap is false because if we get to the end, we need to
                    // return false so that the host continues the search.

                    return SelectNextControl(this, forward, true, true, false) ? HResult.OK : HResult.False;
                }
                finally
                {
                    _select--;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationClient.GetPreferredSize(Size proposedSize, out Size preferredSize)
        {
            preferredSize = new Size();

            try
            {
                preferredSize = GetPreferredSize(proposedSize);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (base.ProcessCmdKey(ref msg, keyData))
                return true;

            NiMessage message = msg;
            bool result = ErrorUtil.ThrowOnFailure(_host.ProcessCmdKey(ref message, keyData));
            msg = message;

            return result;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (base.ProcessDialogKey(keyData))
                return true;

            return ErrorUtil.ThrowOnFailure(_host.ProcessDialogKey(keyData));
        }

        protected override bool ProcessDialogChar(char charCode)
        {
            if (base.ProcessDialogChar(charCode))
                return true;

            return ErrorUtil.ThrowOnFailure(_host.ProcessDialogChar(charCode));
        }
    }
}
