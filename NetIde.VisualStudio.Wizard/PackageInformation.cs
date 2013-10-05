using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    internal class PackageInformation
    {
        public string PackageId { get; private set; }
        public string Version { get; private set; }

        public PackageInformation(string packageId, string version)
        {
            if (packageId == null)
                throw new ArgumentNullException("packageId");
            if (version == null)
                throw new ArgumentNullException("version");

            PackageId = packageId;
            Version = version;
        }
    }
}
