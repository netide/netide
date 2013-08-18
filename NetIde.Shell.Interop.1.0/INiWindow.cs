using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiWindow : IWin32Window
    {
        string Caption { get; set; }
        IResource Icon { get; set; }

        HResult Close();
    }
}
