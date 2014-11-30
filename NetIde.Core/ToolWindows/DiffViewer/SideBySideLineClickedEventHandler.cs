using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class SideBySideLineClickedEventArgs : EventArgs
    {
        public int Line { get; private set; }

        public SideBySideLineClickedEventArgs(int line)
        {
            Line = line;
        }
    }

    internal delegate void SideBySideLineClickedEventHandler(object sender, SideBySideLineClickedEventArgs e);
}
