using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiEditorFactory : ServiceObject, INiEditorFactory
    {
        private IServiceProvider _serviceProvider;

        public HResult SetSite(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return HResult.OK;
        }

        public abstract HResult CreateEditor(out INiWindowPane editor);

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
