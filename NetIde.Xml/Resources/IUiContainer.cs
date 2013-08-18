using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Xml.Resources
{
    public interface IUiContainer
    {
        UiObjectRefCollection Items { get; set; }
    }
}
