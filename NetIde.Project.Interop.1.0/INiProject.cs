using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiProject : INiHierarchy
    {
        HResult AddItem(INiHierarchy location, string file);
        HResult OpenItem(INiHierarchy hier, out INiWindowFrame windowFrame);
        HResult RemoveItem(INiHierarchy hier);
    }
}
