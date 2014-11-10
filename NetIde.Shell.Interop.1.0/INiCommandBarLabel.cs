using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarLabel : INiCommandBarControl
    {
        string Text { get; set; }
    }
}
