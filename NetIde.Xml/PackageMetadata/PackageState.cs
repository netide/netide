using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Xml.PackageMetadata
{
    [Flags]
    public enum PackageState
    {
        Disabled = 1,
        Installed = 2,
        CorePackage = 4,
        UninstallPending = 8,
        UpdatePending = 16,
        InstallPending = 32
    }
}
