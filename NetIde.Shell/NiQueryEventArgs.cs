using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiQueryEventArgs : EventArgs
    {
        public NiCommandStatus Status { get; set; }
    }

    public delegate void NiQueryCallback(NiQueryEventArgs e);
}
