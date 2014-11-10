using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Services.ToolsOptions
{
    internal class PageHost : NativeWindowHost
    {
        private readonly bool _designMode;
        private INiOptionPage _page;

        [Browsable(false)]
        public INiOptionPage Page
        {
            get { return _page; }
            set
            {
                _page = value;

                if (_page == null)
                    ChildHwnd = IntPtr.Zero;
                else
                    ChildHwnd = _page.Handle;
            }
        }

        public PageHost()
        {
            _designMode = ControlUtil.GetIsInDesignMode(this);
        }

        protected override void OnPreMessage(NiPreMessageEventArgs e)
        {
            base.OnPreMessage(e);

            if (_designMode)
                return;

            var preMessageFilter = Page as INiMessageFilter;

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
