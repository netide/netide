using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiMainWindowChrome
    {
        HResult SetEnabled(bool enabled);
        HResult SetColor(int red, int green, int blue);
    }
}
