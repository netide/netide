using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiConnectionPoint
    {
        HResult Advise(object sink, out int cookie);
        HResult Unadvise(int cookie);
    }
}
