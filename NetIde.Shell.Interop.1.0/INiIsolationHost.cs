using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiIsolationHost
    {
        HResult ProcessCmdKey(ref NiMessage message, Keys keyData);

        HResult ProcessDialogKey(Keys keyData);

        HResult ProcessDialogChar(char charData);

        HResult SelectNextControl(bool forward);
    }
}
