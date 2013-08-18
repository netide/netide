using NetIde.Util.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NetIde.Util.Forms
{
    public class TreeView : System.Windows.Forms.TreeView
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            NativeMethods.SendMessage(Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, (IntPtr)NativeMethods.TVS_EX_DOUBLEBUFFER, (IntPtr)NativeMethods.TVS_EX_DOUBLEBUFFER);

            base.OnHandleCreated(e);
        }
    }
}
