using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlRoot("resources", Namespace = Ns.Resources)]
    public class Resources
    {
        [XmlElement("ui")]
        public Ui Ui { get; set; }
    }
}
