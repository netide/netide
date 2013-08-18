using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandTarget
    {
        HResult QueryStatus(Guid command, out NiCommandStatus status);
        HResult Exec(Guid command, object argument, out object result);
    }
}
