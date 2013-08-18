using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    public class NiExecEventArgs : EventArgs
    {
        public object Argument { get; private set; }
        public object Result { get; set; }

        public NiExecEventArgs(object argument)
        {
            Argument = argument;
        }
    }

    public delegate void NiExecCallback(NiExecEventArgs e);
}
