using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("group", Namespace = Ns.Resources)]
    public class Group : UiObject, IUiContainer
    {
        [XmlElement("menu", Type = typeof(Menu))]
        [XmlElement("menuRef", Type = typeof(MenuRef))]
        [XmlElement("button", Type = typeof(Button))]
        [XmlElement("buttonRef", Type = typeof(ButtonRef))]
        [XmlElement("comboBox", Type = typeof(ComboBox))]
        [XmlElement("comboBoxRef", Type = typeof(ComboBoxRef))]
        public UiObjectRefCollection Items { get; set; }

        public Group()
        {
            Items = new UiObjectRefCollection();
        }
    }
}
