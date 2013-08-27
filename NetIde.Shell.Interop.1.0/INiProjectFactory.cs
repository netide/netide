using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiProjectFactory : IServiceProvider, INiObjectWithSite
    {
        HResult CreateProject(string fileName, NiProjectCreateMode createMode, out INiProject project);
    }
}
