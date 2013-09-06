using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.BuildConfiguration
{
    public class TransformResource
    {
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlAttribute("target")]
        public string Target { get; set; }
    }
}
