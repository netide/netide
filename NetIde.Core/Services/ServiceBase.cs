using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NetIde.Shell;

namespace NetIde.Core.Services
{
    internal abstract class ServiceBase : ServiceObject, IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        protected ServiceBase(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            _serviceProvider = serviceProvider;
        }

        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            return _serviceProvider.GetService(serviceType);
        }
    }
}
