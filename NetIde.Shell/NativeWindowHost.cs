using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NativeWindowHost : Control
    {
        private readonly NiConnectionPoint<INiWindowFrameNotify> _connectionPoint = new NiConnectionPoint<INiWindowFrameNotify>();
        private IntPtr _childHwnd;
        private bool _visibleHandled;
        private bool _isShown;
        private bool _activatePending;

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Indicates if arrow keys are accepted as input.")]
        public virtual bool AcceptsArrows { get; set; }

        [DefaultValue(false)]
        [Category("Behavior")]
        [Description("Indicates if tab keys are accepted as input.")]
        public virtual bool AcceptsTab { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IntPtr ChildHwnd
        {
            get { return _childHwnd; }
            set
            {
                if (_childHwnd != value)
                {
                    _childHwnd = value;

                    if (_childHwnd != IntPtr.Zero)
                        ParentChild();
                }
            }
        }

        public event NiPreMessageEventHandler PreMessage;

        protected virtual void OnPreMessage(NiPreMessageEventArgs e)
        {
            var ev = PreMessage;
            if (ev != null)
                ev(this, e);
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

        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down:
                    if (AcceptsArrows)
                        return true;
                    break;

                case Keys.Tab:
                    if (AcceptsTab)
                        return true;
                    break;
            }

            return base.IsInputKey(keyData);
        }

        public override bool PreProcessMessage(ref Message msg)
        {
            var e = new NiPreMessageEventArgs
            {
                Message = msg
            };

            OnPreMessage(e);

            if (e.Handled)
            {
                msg = e.Message;
                return true;
            }

            return base.PreProcessMessage(ref msg);
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
}
