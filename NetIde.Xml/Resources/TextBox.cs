using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("textBox", Namespace = Ns.Resources)]
    public class TextBox : UiControl
    {
        [XmlAttribute("style")]
        public TextBoxStyle Style { get; set; }
    }
}
