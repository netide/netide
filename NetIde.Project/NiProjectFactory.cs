using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public abstract class NiProjectFactory : ServiceObject, INiProjectFactory
    {
        private IServiceProvider _serviceProvider;

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

        public abstract HResult CreateProject(string fileName, NiProjectCreateMode createMode, out INiProject project);

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
