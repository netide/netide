using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.Services.Finder
{
    internal class NiFinder : ServiceBase, INiFinder
    {
        private readonly CorePackage _package;
        private FindForm _form;

        public NiFinder(CorePackage package)
            : base(package)
        {
            _package = package;
        }

        public HResult OpenDialog(NiFindOptions options, NiFindOptions optionsMask)
        {
            try
            {
                if (_form != null && _form.IsDisposed)
                    _form = null;

                bool show = _form == null;

                if (_form == null)
                {
                    // We need to set the site early because SetOptions depends
                    // on this.

                    _form = new FindForm(_package)
                    {
                        Site = new SiteProxy(this)
                    };
                }

                _form.SetOptions(options, optionsMask);

                if (show)
                    _form.Show(((INiEnv)GetService(typeof(INiEnv))).MainWindow);
                else
                    _form.Activate();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
