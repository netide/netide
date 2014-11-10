using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("button", Namespace = Ns.Resources)]
    public class Button : UiControl
    {
        [XmlAttribute("text")]
        public string Text { get; set; }

        [XmlAttribute("image")]
        public string Image { get; set; }

        [XmlAttribute("key")]
        public string Key { get; set; }

        [XmlAttribute("style")]
        public DisplayStyle Style { get; set; }
    }
}
