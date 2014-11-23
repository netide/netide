using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Util.Forms
{
    public class BrowseButtonEventArgs : EventArgs
    {
        public BrowseButtonEventArgs(BrowseButton button)
        {
            Button = button;
        }

        public BrowseButton Button { get; private set; }

        public bool Handled { get; set; }
    }

    public delegate void BrowseButtonEventHandler(object sender, BrowseButtonEventArgs e);
}
