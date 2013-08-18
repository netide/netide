using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Util.Forms
{
    public class CancelPreviewEventArgs
    {
        public bool Handled { get; set; }

        public CancelPreviewEventArgs()
        {
            Handled = false;
        }
    }

    public delegate void CancelPreviewEventHandler(object sender, CancelPreviewEventArgs e);
}
