using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiOptionPage : NiWindow, INiOptionPage
    {
        private IServiceProvider _serviceProvider;

        public event EventHandler Activate;

        protected virtual void OnActivate(EventArgs e)
        {
            var ev = Activate;
            if (ev != null)
                ev(this, e);
        }

        public event CancelEventHandler Deactivate;

        protected virtual void OnDeactivate(CancelEventArgs e)
        {
            var ev = Deactivate;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler Apply;

        protected virtual void OnApply(EventArgs e)
        {
            var ev = Apply;
            if (ev != null)
                ev(this, e);
        }

        public event EventHandler Cancel;

        protected virtual void OnCancel(EventArgs e)
        {
            var ev = Cancel;
            if (ev != null)
                ev(this, e);
        }

        HResult INiOptionPage.Activate()
        {
            try
            {
                OnActivate(EventArgs.Empty);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiOptionPage.Deactivate(out bool canDeactivate)
        {
            canDeactivate = false;

            try
            {
                var e = new CancelEventArgs();

                OnDeactivate(e);

                canDeactivate = !e.Cancel;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiOptionPage.Apply()
        {
            try
            {
                OnApply(EventArgs.Empty);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        HResult INiOptionPage.Cancel()
        {
            try
            {
                OnCancel(EventArgs.Empty);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetSite(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return HResult.OK;
        }

        public HResult GetSite(out IServiceProvider serviceProvider)
        {
            serviceProvider = _serviceProvider;
            return HResult.OK;
        }

        public abstract HResult Initialize();

        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
