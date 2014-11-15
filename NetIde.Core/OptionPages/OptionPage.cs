using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.OptionPages
{
    internal class OptionPage<T> : NiOptionPage
        where T : OptionPageControl, new()
    {
        public T Page
        {
            get { return (T)Controls[0]; }
        }

        public override HResult Initialize()
        {
            try
            {
                Controls.Add(new T
                {
                    Site = new SiteProxy(this),
                    Dock = DockStyle.Fill
                });

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

            Page.RaiseActivate();
        }

        protected override void OnDeactivate(CancelEventArgs e)
        {
            base.OnDeactivate(e);

            e.Cancel = Page.RaiseDeactivate() || e.Cancel;
        }

        protected override void OnApply(EventArgs e)
        {
            base.OnApply(e);

            Page.RaiseApply();
        }

        protected override void OnCancel(EventArgs e)
        {
            base.OnCancel(e);

            Page.RaiseCancel();
        }
    }
}
