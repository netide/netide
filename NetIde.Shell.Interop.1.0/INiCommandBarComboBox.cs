using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarComboBox : INiCommandBarControl
    {
        Guid FillCommand { get; }
        NiCommandBarComboBoxStyle Style { get; set; }
        string[] Values { get; set; }
        string SelectedValue { get; set; }
    }
}
