using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiCommandStatus
    {
        Supported = 1,
        Enabled = 2,
        Latched = 4,
        Invisible = 8
    }
}
