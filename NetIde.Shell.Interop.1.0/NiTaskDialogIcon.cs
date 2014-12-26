using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiTaskDialogIcon
    {
        None = 0,
        Warning = 1 << 0,
        Error = 1 << 1,
        Information = 1 << 2,
        Shield = 1 << 3
    }
}
