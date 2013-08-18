using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    public class UiObjectRef
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("guid")]
        public Guid Guid { get; set; }
    }
}
