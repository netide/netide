using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiMessageFilter : INiPreMessageFilter
    {
        HResult IsInputKey(Keys keyData, out bool result);
        HResult IsInputChar(char charCode, out bool result);
    }
}
