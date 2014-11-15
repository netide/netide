using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiIsolationClient : IWin32Window, IDisposable
    {
        HResult SetHost(INiIsolationHost host);

        HResult PreviewKeyDown(Keys keyData);

        HResult PreProcessMessage(ref NiMessage message, out PreProcessMessageResult preProcessMessageResult);

        HResult ProcessMnemonic(char charCode);

        HResult SelectNextControl(bool forward);

        HResult GetPreferredSize(Size proposedSize, out Size preferredSize);
    }
}
