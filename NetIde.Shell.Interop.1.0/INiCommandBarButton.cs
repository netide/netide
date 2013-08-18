using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarButton : INiCommandBarControl
    {
        NiCommandDisplayStyle DisplayStyle { get; set; }
        IResource Image { get; set; }
        Keys ShortcutKeys { get; set; }
        bool IsLatched { get; set; }
    }
}
