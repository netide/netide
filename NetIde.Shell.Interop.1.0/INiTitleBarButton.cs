using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTitleBarButton
    {
        HResult GetImage(out IResource image);
        HResult GetPriority(out int priority);
        HResult GetForeColor(out Color? foreColor);
        HResult GetBackColor(out Color? backColor);
        HResult GetEnabled(out bool enabled);
        HResult GetVisible(out bool visible);
    }
}
