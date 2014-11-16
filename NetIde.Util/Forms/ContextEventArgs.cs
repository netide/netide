using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Util.Forms
{
    public class ContextEventArgs : EventArgs
    {
        public Point Location { get; private set; }

        public bool Handled { get; set; }

        public int X
        {
            get { return Location.X; }
        }

        public int Y
        {
            get { return Location.Y; }
        }

        public ContextEventArgs(Point location)
        {
            Location = location;
        }
    }

    public delegate void ContextEventHandler(object sender, ContextEventArgs e);
}
