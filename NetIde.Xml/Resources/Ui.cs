using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType(Namespace = Ns.Resources)]
    public class Ui : IUiContainer
    {
        [XmlElement("menu", Type = typeof(Menu))]
        [XmlElement("menuRef", Type = typeof(MenuRef))]
        [XmlElement("group", Type = typeof(Group))]
        [XmlElement("groupRef", Type = typeof(GroupRef))]
        public UiObjectRefCollection Items { get; set; }

        public Ui()
        {
            Items = new UiObjectRefCollection();
        }
    }
}
