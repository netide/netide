using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiWindowPaneSelection : INiConnectionPoint
    {
        INiWindowPane ActiveDocument { get; }

        HResult Advise(INiWindowPaneSelectionNotify sink, out int cookie);
    }
}
