using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Shell
{
    public abstract class NiWindowHost : Control
    {
        private readonly NiConnectionPoint<INiWindowFrameNotify> _connectionPoint = new NiConnectionPoint<INiWindowFrameNotify>();
        private IntPtr _childHwnd;
        private bool _visibleHandled;
        private bool _isShown;
        private bool _activatePending;
        private readonly bool _designMode;
        private INiWindowPane _window;

        public INiWindowPane Window
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

        protected NiWindowHost()
        {
            _designMode = ControlUtil.GetIsInDesignMode(this);
        }

        protected abstract INiWindowPane CreateWindow();

        protected void SetChildHwnd(IntPtr handle)
        {
            _childHwnd = handle;

            if (_childHwnd != IntPtr.Zero)
                ParentChild();
        }

        private void ParentChild()
        {
            SetParent(new HandleRef(this, _childHwnd), new HandleRef(this, Handle));
            PerformLayout();
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (_childHwnd == IntPtr.Zero)
                return;

            var size = ClientSize;

            SetWindowPos(
                new HandleRef(this, _childHwnd),
                new HandleRef(),
                0,
                0,
                size.Width,
                size.Height,
                SWP_NOACTIVATE | SWP_NOZORDER | SWP_NOMOVE
            );
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            SetFocus(new HandleRef(this, _childHwnd));
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && !_visibleHandled)
            {
                _visibleHandled = true;

                RaiseShow(NiWindowShow.Show);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _connectionPoint.ForAll(p => p.OnSize());
        }

        public void RaiseShow(NiWindowShow show)
        {
            if (show == NiWindowShow.Show)
                _isShown = true;

            if (show == NiWindowShow.Activate)
            {
                if (!_isShown)
                {
                    _activatePending = true;
                    return;
                }

                _activatePending = false;
            }

            if (show == NiWindowShow.Deactivate && _activatePending)
            {
                _activatePending = false;
                return;
            }

            _connectionPoint.ForAll(p => p.OnShow(show));

            if (show == NiWindowShow.Show && _activatePending)
                RaiseShow(NiWindowShow.Activate);
        }

        public void RaiseClose(NiFrameCloseMode closeMode, ref bool cancel)
        {
            bool notifyCancel = cancel;

            _connectionPoint.ForAll(p => p.OnClose(closeMode, ref notifyCancel));

            cancel = notifyCancel;
        }

        protected override void Dispose(bool disposing)
        {
            RaiseShow(NiWindowShow.Close);

            base.Dispose(disposing);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (_designMode || _window != null)
                return;

            _window = CreateWindow();

            SetChildHwnd(_window.Handle);

            OnWindowCreated(EventArgs.Empty);
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

        public override bool PreProcessMessage(ref Message msg)
        {
            bool processed;

            var preMessageFilter = _window as INiMessageFilter;
            if (preMessageFilter != null)
            {
                NiMessage message = msg;

                ErrorUtil.ThrowOnFailure(preMessageFilter.PreFilterMessage(ref message, out processed));

                if (processed)
                    msg = message;
            }
            else
            {
                processed = base.PreProcessMessage(ref msg);
            }

            return processed;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            bool result;

            var preMessageFilter = _window as INiMessageFilter;
            if (preMessageFilter != null)
                ErrorUtil.ThrowOnFailure(preMessageFilter.IsInputKey(keyData, out result));
            else
                result = base.IsInputKey(keyData);

            return result;
        }

        protected override bool IsInputChar(char charCode)
        {
            bool result;

            var preMessageFilter = _window as INiMessageFilter;
            if (preMessageFilter != null)
                ErrorUtil.ThrowOnFailure(preMessageFilter.IsInputChar(charCode, out result));
            else
                result = base.IsInputChar(charCode);

            return result;
        }

        private const int SWP_NOACTIVATE = 0x0010;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_NOMOVE = 0x0002;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(HandleRef hWndChild, HandleRef hWndNewParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SetFocus(HandleRef hWnd);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy, int flags);
    }

    public abstract class NiWindowHost<T> : NiWindowHost
        where T : class, INiWindowPane
    {
        public new T Window
        {
            get { return (T)base.Window; }
        }
    }
}
