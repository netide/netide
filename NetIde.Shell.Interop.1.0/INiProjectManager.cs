using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiProjectManager : INiConnectionPoint
    {
        INiProject ActiveProject { get; }

        HResult Advise(INiProjectManagerNotify sink, out int cookie);
        HResult OpenProject(string fileName);
        HResult OpenProjectViaDialog(string startDirectory);
        HResult RegisterProjectFactory(Guid guid, INiProjectFactory projectFactory);
    }
}
