using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("label", Namespace = Ns.Resources)]
    public class Label : UiControl
    {
        [XmlAttribute("text")]
        public string Text { get; set; }
    }
}
