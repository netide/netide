using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class MruHistoryEventArgs : EventArgs
    {
        public IList<string> History { get; private set; }

        public MruHistoryEventArgs(IList<string> history)
        {
            if (history == null)
                throw new ArgumentNullException("history");

            History = history;
        }
    }

    public delegate void MruHistoryEventHandler(object sender, MruHistoryEventArgs e);
}
