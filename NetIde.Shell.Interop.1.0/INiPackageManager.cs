using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiPackageManager
    {
        HResult OpenPackageManager();
        HResult GetInstallationPath(INiPackage package, out string path);
    }
}
