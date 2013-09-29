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
        SystemPackage = 8,
        UninstallPending = 16,
        UpdatePending = 32,
        InstallPending = 64
    }
}
