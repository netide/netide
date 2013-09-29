using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Setup
{
    public class SetupPackage
    {
        public PackageMetadata Metadata { get; private set; }

        public bool IsInConfiguration { get; private set; }

        public string InstalledVersion { get; private set; }

        public SetupPackage(PackageMetadata metadata, bool isInConfiguration, string installedVersion)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Metadata = metadata;
            IsInConfiguration = isInConfiguration;
            InstalledVersion = installedVersion;
        }

        public override string ToString()
        {
            return String.Format("{0}, InConfiguration={1}, InstalledVersion={2}", Metadata, IsInConfiguration, InstalledVersion);
        }
    }
}
