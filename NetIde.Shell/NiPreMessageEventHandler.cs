using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiPreMessageEventArgs : EventArgs
    {
        public NiMessage Message { get; set; }
        public bool Handled { get; set; }
    }

    public delegate void NiPreMessageEventHandler(object sender, NiPreMessageEventArgs e);
}
