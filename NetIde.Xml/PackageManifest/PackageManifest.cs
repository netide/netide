using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.PackageManifest
{
    [XmlRoot("manifest", Namespace = Ns.PackageManifest)]
    public class PackageManifest
    {
        public const string FileName = "NiPackage.manifest";

        [XmlElement("entryPoint")]
        public string EntryPoint { get; set; }
    }
}
