using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiTextBufferState
    {
        None = 0,
        ReadOnly = 1,
        Dirty = 2
    }
}
