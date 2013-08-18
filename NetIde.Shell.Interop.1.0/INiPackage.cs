using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiPackage : IServiceProvider, IDisposable
    {
        HResult SetSite(IServiceProvider serviceProvider);
        HResult Initialize();
        HResult GetStringResource(string key, out string value);
        HResult GetNiResources(out IResource value);
        HResult CreateToolWindow(Guid guid, out INiWindowPane toolWindow);
        HResult QueryClose(out bool canClose);
        HResult Register(INiRegistrationContext registrationContext);
        HResult Unregister(INiRegistrationContext registrationContext);
    }
}
