using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Shell
{
    public abstract class NiIsolationHost : Control, INiIsolationHost
    {
        private IntPtr _childHwnd;
        private readonly bool _designMode;
        private INiIsolationClient _window;
        private int _select;

        public INiIsolationClient Window
        {
            get
            {
                if (_window == null)
                    CreateHandle();

                return _window;
            }
        }

        public event EventHandler WindowCreated;

        protected virtual void OnWindowCreated(EventArgs e)
        {
            var ev = WindowCreated;
            if (ev != null)
                ev(this, e);
        }

        protected NiIsolationHost()
        {
            SetStyle(ControlStyles.Selectable, true);

            _designMode = ControlUtil.GetIsInDesignMode(this);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        protected abstract INiIsolationClient CreateWindow();

        protected void SetChildHwnd(IntPtr handle)
        {
            _childHwnd = handle;

            if (_childHwnd != IntPtr.Zero)
                ParentChild();
        }

        private void ParentChild()
        {
            NativeMethods.SetParent(new HandleRef(this, _childHwnd), new HandleRef(this, Handle));

            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (_childHwnd == IntPtr.Zero)
                return;

            var size = ClientSize;

            NativeMethods.SetWindowPos(
                new HandleRef(this, _childHwnd),
                new HandleRef(),
                0,
                0,
                size.Width,
                size.Height,
                NativeMethods.SWP_NOACTIVATE | NativeMethods.SWP_NOZORDER | NativeMethods.SWP_NOMOVE
            );
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            NativeMethods.SetFocus(new HandleRef(this, _childHwnd));
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (_designMode || _window != null)
                return;

            _window = CreateWindow();

            if (_window == null)
                throw new InvalidOperationException("CreateWindow did not create a client");

            _window.SetHost(this);

            SetChildHwnd(_window.Handle);

            OnWindowCreated(EventArgs.Empty);

            SetBoundsCore(0, 0, Width, Height, BoundsSpecified.Size);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            if (_window != null)
            {
                _window.Dispose();
                _window = null;
            }
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = ErrorUtil.ThrowOnFailure(_window.PreviewKeyDown(e.KeyData));
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            NiMessage message = msg;
            PreProcessMessageResult preProcessMessageResult;
            bool processed = ErrorUtil.ThrowOnFailure(_window.PreProcessMessage(ref message, out preProcessMessageResult));
            msg = message;

            ControlStubs.ControlSetState2(
                this,
                ControlStubs.STATE2_INPUTKEY,
                (preProcessMessageResult & PreProcessMessageResult.IsInputKey) != 0
            );

            ControlStubs.ControlSetState2(
                this,
                ControlStubs.STATE2_INPUTCHAR,
                (preProcessMessageResult & PreProcessMessageResult.IsInputChar) != 0
            );

            return processed;
        }

        protected override bool ProcessMnemonic(char charCode)
        {
            return ErrorUtil.ThrowOnFailure(_window.ProcessMnemonic(charCode));
        }

        protected override void Select(bool directed, bool forward)
        {
            if (_select > 0)
                return;

            _select++;

            try
            {
                // We were the target of select next control. Forward the
                // call to the isolation client which does its search. If it
                // matches a control, we need to make ourselves active.

                if (ErrorUtil.ThrowOnFailure(_window.SelectNextControl(!directed || forward)))
                {
                    base.Select(directed, forward);
                    return;
                }

                // If the client wasn't able to select something, we continue the
                // search from here. One small detail is that SelectNextControl
                // does not match itself. When it would match an IsolationClient,
                // this would mean that the search does not go into the
                // IsolationHost. We specifically match this case by first doing
                // a non-wrapping search and matching the root for IsolationClient.
                // If that matches, we allow the IsolationClient to continue
                // the search upwards. Otherwise, we continue the search
                // from the root as usual.

                var root = ControlUtil.GetRoot(this);

                if (root.SelectNextControl(this, !directed || forward, true, true, !(root is NiIsolationClient)))
                    return;

                if (root is NiIsolationClient)
                    ControlStubs.ControlSelect(root, directed, forward);
            }
            finally
            {
                _select--;
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (_window != null && (specified & BoundsSpecified.Size) != 0)
            {
                var size = GetPreferredSize(new Size(width, height));

                width = size.Width;
                height = size.Height;
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            ErrorUtil.ThrowOnFailure(_window.GetPreferredSize(proposedSize, out preferredSize));

            return preferredSize;
        }

        HResult INiIsolationHost.ProcessCmdKey(ref NiMessage message, Keys keyData)
        {
            try
            {
                Message msg = message;
                bool result = ProcessCmdKey(ref msg, keyData);
                message = msg;

                return result ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationHost.ProcessDialogKey(Keys keyData)
        {
            try
            {
                return ProcessDialogKey(keyData) ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationHost.ProcessDialogChar(char charData)
        {
            try
            {
                return ProcessDialogChar(charData) ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiIsolationHost.SelectNextControl(bool forward)
        {
            try
            {
                if (_select > 0)
                    return HResult.False;

                _select++;

                try
                {
                    // The client was the target of select next control. It
                    // has handed the search over to us and we continue from
                    // here. We need to wrap to get the correct behavior.

                    return ControlUtil.GetRoot(this).SelectNextControl(this, forward, true, true, true) ? HResult.OK : HResult.False;
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
    }
}
