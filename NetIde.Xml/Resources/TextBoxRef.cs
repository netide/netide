using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NetIde.Xml.Resources
{
    [XmlType("textBoxRef", Namespace = Ns.Resources)]
    public class TextBoxRef : UiObjectRef
    {
    }
}
