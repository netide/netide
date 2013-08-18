using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.PackageMetadata
{
    [XmlRoot("package", Namespace = Ns.NuSpec)]
    public class NuSpec
    {
        [XmlElement("metadata")]
        public PackageMetadata Metadata { get; set; }
    }
}
