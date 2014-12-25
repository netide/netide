using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiProjectExplorer
    {
        HResult Show();
        HResult Hide();
        HResult GetSelectedHierarchy(out INiHierarchy hier);
    }
}
