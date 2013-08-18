using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    public abstract class ServiceObject : MarshalByRefObject, IDisposable
    {
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
