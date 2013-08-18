using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.OptionPages
{
    internal class OptionPage<T> : NiOptionPage
        where T : OptionPageControl, new()
    {
        public new T Window
        {
            get { return (T)base.Window; }
            set { base.Window = value; }
        }

        public override HResult Initialize()
        {
            try
            {
                Window = new T
                {
                    Site = new SiteProxy(this)
                };

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void OnActivate(EventArgs e)
        {
            base.OnActivate(e);

            Window.RaiseActivate();
        }

        protected override void OnDeactivate(CancelEventArgs e)
        {
            base.OnDeactivate(e);

            e.Cancel = Window.RaiseDeactivate() || e.Cancel;
        }

        protected override void OnApply(EventArgs e)
        {
            base.OnApply(e);

            Window.RaiseApply();
        }

        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);

            Window.RaiseCancel();
        }
    }
}
