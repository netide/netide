using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("menu", Namespace = Ns.Resources)]
    public class Menu : UiObject, IUiContainer
    {
        [XmlElement("group", Type = typeof(Group))]
        [XmlElement("groupRef", Type = typeof(GroupRef))]
        public UiObjectRefCollection Items { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }

        [XmlAttribute("image")]
        public string Image { get; set; }

        [XmlAttribute("kind")]
        public MenuKind Kind { get; set; }

        [XmlAttribute("style")]
        public DisplayStyle Style { get; set; }

        public Menu()
        {
            Items = new UiObjectRefCollection();
        }
    }
}
