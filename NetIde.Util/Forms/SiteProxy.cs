using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class SiteProxy : ISite
    {
        private readonly IServiceProvider _serviceProvider;

        public IComponent Component
        {
            get { return null; }
        }

        public IContainer Container
        {
            get { return null; }
        }

        public bool DesignMode
        {
            get { return false; }
        }

        public string Name { get; set; }

        public SiteProxy(IServiceProvider serviceProvider)
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
