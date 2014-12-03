using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal class DiffLineClickedEventArgs : EventArgs
    {
        public int Line { get; private set; }

        public DiffLineClickedEventArgs(int line)
        {
            Line = line;
        }
    }

    internal delegate void DiffLineClickedEventHandler(object sender, DiffLineClickedEventArgs e);
}
