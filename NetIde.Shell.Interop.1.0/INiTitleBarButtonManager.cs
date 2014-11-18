using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTitleBarButtonManager : INiConnectionPoint
    {
        HResult Advise(INiTitleBarButtonManagerNotify sink, out int cookie);
        HResult AddButton(INiTitleBarButton button, out int cookie);
        HResult UpdateButton(int cookie, INiTitleBarButton button);
        HResult RemoveButton(int cookie);
    }
}
