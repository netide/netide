using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class NiWindowShowEventArgs : EventArgs
    {
        public NiWindowShow Action { get; private set; }

        public NiWindowShowEventArgs(NiWindowShow action)
        {
            Action = action;
        }
    }

    public delegate void NiWindowShowEventHandler(object sender, NiWindowShowEventArgs e);
}
