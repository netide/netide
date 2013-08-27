using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public abstract class NiProject : NiHierarchy, INiProject
    {
        public virtual HResult AddItem(INiHierarchy location, string file)
        {
            throw new NotImplementedException();
        }

        public virtual HResult OpenItem(INiHierarchy hier, out INiWindowFrame windowFrame)
        {
            throw new NotImplementedException();
        }

        public virtual HResult RemoveItem(INiHierarchy hier)
        {
            throw new NotImplementedException();
        }
    }
}
