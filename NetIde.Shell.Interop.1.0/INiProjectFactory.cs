using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiProjectFactory : IServiceProvider
    {
        HResult SetSite(IServiceProvider serviceProvider);
        HResult CreateProject(string fileName, NiProjectCreateMode createMode, out INiProject project);
    }
}
