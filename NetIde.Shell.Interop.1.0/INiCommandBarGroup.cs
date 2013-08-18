using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarGroup : IDisposable
    {
        Guid Id { get; }
        int Priority { get; }
        INiList<INiCommandBarControl> Controls { get; }
    }
}
