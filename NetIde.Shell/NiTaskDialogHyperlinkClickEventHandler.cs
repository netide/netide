using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogHyperlinkClickEventArgs : NiTaskDialogEventArgs
    {
        public string Hyperlink { get; private set; }

        public NiTaskDialogHyperlinkClickEventArgs(INiActiveTaskDialog activeTaskDialog, string hyperlink)
            : base(activeTaskDialog)
        {
            Hyperlink = hyperlink;
        }
    }

    public delegate void NiTaskDialogHyperlinkClickEventHandler(object sender, NiTaskDialogHyperlinkClickEventArgs e);
}
