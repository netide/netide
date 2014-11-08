using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.WaitDialog
{
    internal class NiWaitDialogFactory : ServiceBase, INiWaitDialogFactory
    {
        private readonly SynchronizationContext _synchronizationContext;

        public NiWaitDialogFactory(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public HResult CreateInstance(out INiWaitDialog result)
        {
            result = null;

            try
            {
                result = new NiWaitDialog(this, _synchronizationContext);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
