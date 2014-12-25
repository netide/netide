using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiProjectManager : INiConnectionPoint
    {
        INiProject ActiveProject { get; }

        HResult Advise(INiProjectManagerNotify sink, out int cookie);
        HResult CreateProject(string fileName);
        HResult CreateProjectViaDialog(string startDirectory);
        HResult OpenProject(string fileName);
        HResult OpenProjectViaDialog(string startDirectory);
        HResult RegisterProjectFactory(Guid guid, INiProjectFactory projectFactory);
        HResult CloseProject();
    }
}
