using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiProject : INiHierarchy
    {
        HResult AddItem(INiHierarchy location, string file);
        HResult OpenItem(INiHierarchy hier, out INiWindowFrame windowFrame);
        HResult RemoveItem(INiHierarchy hier);
    }
}
