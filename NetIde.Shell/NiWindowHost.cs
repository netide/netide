using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiWindowHost : NiIsolationHost
    {
        private readonly NiConnectionPoint<INiWindowFrameNotify> _connectionPoint = new NiConnectionPoint<INiWindowFrameNotify>();
        private bool _visibleHandled;
        private bool _isShown;
        private bool _activatePending;

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
