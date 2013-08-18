using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("comboBox", Namespace = Ns.Resources)]
    public class ComboBox : UiControl
    {
        [XmlAttribute("style")]
        public ComboBoxStyle Style { get; set; }

        [XmlAttribute("fillCommandGuid")]
        public Guid FillCommandGuid { get; set; }

        [XmlAttribute("fillCommandId")]
        public string FillCommandId { get; set; }
    }
}
