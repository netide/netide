using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.LocalRegistry
{
    internal class Settings : NiSettings
    {
        public Settings(INiPackage package)
            : base(package)
        {
        }
    }
}
