using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Project.Interop
{
    public interface INiHierarchyNotify
    {
        void OnChildAdded(INiHierarchy hier);
        void OnChildRemoved(INiHierarchy hier);
        void OnPropertyChanged(INiHierarchy hier, int property);
    }
}
