using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiProjectFactory : IServiceProvider, INiObjectWithSite
    {
        HResult CreateProject(string fileName, NiProjectCreateMode createMode, out INiProject project);
    }
}
