using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("menuRef", Namespace = Ns.Resources)]
    public class MenuRef : UiObjectRef, IUiContainer
    {
        [XmlElement("group", Type = typeof(Group))]
        [XmlElement("groupRef", Type = typeof(GroupRef))]
        public UiObjectRefCollection Items { get; set; }

        public MenuRef()
        {
            Items = new UiObjectRefCollection();
        }
    }
}
