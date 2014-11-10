using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarWindow : INiWindowPane
    {
        HResult GetPreferredSize(Size proposedSize, out Size size);
    }
}
