using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Project.Interop
{
    public interface INiProjectManagerNotify
    {
        void OnActiveProjectChanged();
    }
}
