using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.BuildConfiguration
{
    public class BuildNuGetPackage
    {
        [XmlAttribute("manifest")]
        public string Manifest { get; set; }

        [XmlAttribute("basePath")]
        public string BasePath { get; set; }

        [XmlAttribute("target")]
        public string Target { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }
    }
}
