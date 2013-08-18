using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum StreamState
    {
        None = 0,
        Readable = 1,
        Writable = 2,
        Seekable = 4
    }
}
