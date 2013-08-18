using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiOptionPage : IWin32Window
    {
        HResult SetSite(IServiceProvider serviceProvider);
        HResult Initialize();
        HResult Activate();
        HResult Deactivate(out bool canDeactivate);
        HResult Apply();
        HResult Cancel();
    }
}
