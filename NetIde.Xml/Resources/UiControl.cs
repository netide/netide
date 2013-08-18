using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    public class UiControl : UiObject
    {
        [XmlAttribute("tooltip")]
        public string ToolTip { get; set; }
    }
}
