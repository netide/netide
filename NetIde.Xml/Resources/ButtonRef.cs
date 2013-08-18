using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("buttonRef", Namespace = Ns.Resources)]
    public class ButtonRef : UiObjectRef
    {
    }
}
