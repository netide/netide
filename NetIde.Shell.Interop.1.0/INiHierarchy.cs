using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiHierarchy : INiConnectionPoint, IServiceProvider, INiObjectWithSite
    {
        HResult Advise(INiHierarchyNotify sink, out int cookie);
        HResult GetProperty(int property, out object value);
        HResult SetProperty(int property, object value);
        HResult Close();
    }
}
