using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.BuildConfiguration
{
    public class InstallPackage
    {
        [XmlAttribute("context")]
        public string Context { get; set; }

        [XmlAttribute("package")]
        public string Package { get; set; }
    }
}
