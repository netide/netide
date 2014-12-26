using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogButtonClickEventArgs : NiTaskDialogEventArgs
    {
        public NiTaskDialogButton Button { get; private set; }

        public bool Close { get; set; }

        public NiTaskDialogButtonClickEventArgs(INiActiveTaskDialog activeTaskDialog, NiTaskDialogButton button)
            : base(activeTaskDialog)
        {
            Button = button;
        }
    }

    public delegate void NiTaskDialogButtonClickEventHandler(object sender, NiTaskDialogButtonClickEventArgs e);
}
