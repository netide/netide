using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    public class UiObject : UiObjectRef
    {
        [XmlAttribute("priority")]
        public int Priority { get; set; }

        [XmlAttribute("text")]
        public string Text { get; set; }
    }
}
