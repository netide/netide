using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public enum NiHierarchyProperty
    {
        Parent = -1,
        FirstChild = -2,
        NextSibling = -3,
        Image = -4,
        OverlayImage = -5,
        Name = -6,
        SortPriority = -7,
        ItemType = -8,
        ContainingProject = -9,
        Root = -10,
        OwnerType = -11
    }
}
