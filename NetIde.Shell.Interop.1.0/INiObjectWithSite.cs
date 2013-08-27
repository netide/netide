using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiObjectWithSite
    {
        HResult SetSite(IServiceProvider serviceProvider);
        HResult GetSite(out IServiceProvider serviceProvider);
    }
}
