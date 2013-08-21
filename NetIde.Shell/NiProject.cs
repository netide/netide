using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiProject : NiHierarchy, INiProject
    {
        public abstract HResult AddItem(INiHierarchy location, string file);

        public abstract HResult OpenItem(INiHierarchy item, out INiWindowFrame windowFrame);

        public abstract HResult RemoveItem(INiHierarchy item);
    }
}
