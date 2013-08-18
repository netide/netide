using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiLogEvent
    {
        DateTime TimeStamp { get;  }
        string Content { get; }
        string Message { get; }
        int Severity { get; }
        string Source { get; }
    }
}
