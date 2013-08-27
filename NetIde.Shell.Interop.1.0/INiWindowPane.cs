using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiWindowPane : IServiceProvider, IDisposable, IWin32Window, INiObjectWithSite
    {
        HResult Initialize();
    }
}
