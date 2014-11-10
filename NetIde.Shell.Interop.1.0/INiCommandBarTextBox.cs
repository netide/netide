using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarTextBox : INiCommandBarControl
    {
        NiCommandBarTextBoxStyle Style { get; set; }
        string Value { get; set; }
    }
}
