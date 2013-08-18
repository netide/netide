using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Context
{
    [XmlRoot("context", Namespace = Ns.Context)]
    public class Context
    {
        public const string FileName = "NiContext.xml";

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("nuGetSite")]
        public string NuGetSite { get; set; }
    }
}
