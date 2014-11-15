using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum PreProcessMessageResult
    {
        None = 0,
        IsInputKey = 1,
        IsInputChar = 2
    }
}
