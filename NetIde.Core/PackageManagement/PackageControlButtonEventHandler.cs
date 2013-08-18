using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Core.PackageManagement
{
    internal class PackageControlButtonEventArgs : EventArgs
    {
        public PackageMetadata Package { get; private set; }
        public PackageControlButton Button { get; private set; }

        public PackageControlButtonEventArgs(PackageMetadata package, PackageControlButton button)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            Package = package;
            Button = button;
        }
    }

    internal delegate void PackageControlButtonEventHandler(object sender, PackageControlButtonEventArgs e);
}
