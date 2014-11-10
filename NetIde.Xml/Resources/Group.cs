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
        [XmlElement("textBox", Type = typeof(TextBox))]
        [XmlElement("textBoxRef", Type = typeof(TextBoxRef))]
        [XmlElement("label", Type = typeof(Label))]
        [XmlElement("labelRef", Type = typeof(LabelRef))]
        public UiObjectRefCollection Items { get; set; }

        [XmlAttribute("align")]
        public GroupAlign Align { get; set; }

        public Group()
        {
            Items = new UiObjectRefCollection();
        }
    }
}
