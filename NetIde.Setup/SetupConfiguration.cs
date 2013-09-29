using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Setup
{
    public class SetupConfiguration
    {
        public string InstallationPath { get; set; }

        public string StartMenu { get; set; }

        public bool CreateStartMenuShortcut { get; set; }

        public bool CreateDesktopShortcut { get; set; }

        public IList<SetupPackage> Packages { get; private set; }

        public SetupMode Mode { get; set; }

        public SetupConfiguration()
        {
            Packages = new List<SetupPackage>();
            CreateStartMenuShortcut = true;
            CreateDesktopShortcut = true;
        }
    }
}
