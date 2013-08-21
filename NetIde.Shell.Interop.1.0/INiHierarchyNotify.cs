using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiHierarchyNotify
    {
        void OnChildAdded(INiHierarchy item);
        void OnChildRemoved(INiHierarchy item);
        void OnPropertyChanged(INiHierarchy item, int property);
    }
}
