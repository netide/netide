using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Shell
{
    public abstract class NiWindowHost<T> : NativeWindowHost
        where T : class, INiWindowPane
    {
        private readonly bool _designMode;
        private T _window;

        public T Window
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

        protected abstract T CreateWindow();

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (_designMode || _window != null)
                return;

            _window = CreateWindow();

            ChildHwnd = _window.Handle;

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

        protected override void OnPreMessage(NiPreMessageEventArgs e)
        {
            base.OnPreMessage(e);

            if (_designMode)
                return;

            var preMessageFilter = _window as INiPreMessageFilter;

            if (preMessageFilter == null)
                return;

            var message = e.Message;

            bool handled;
            ErrorUtil.ThrowOnFailure(preMessageFilter.PreFilterMessage(ref message, out handled));

            e.Handled = handled;

            if (e.Handled)
                e.Message = message;
        }
    }
}
