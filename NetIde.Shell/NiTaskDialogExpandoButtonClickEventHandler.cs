using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogExpandoButtonClickEventArgs : NiTaskDialogEventArgs
    {
        public bool Expanded { get; private set; }

        public NiTaskDialogExpandoButtonClickEventArgs(INiActiveTaskDialog activeTaskDialog, bool expanded)
            : base(activeTaskDialog)
        {
            Expanded = expanded;
        }
    }

    public delegate void NiTaskDialogExpandoButtonClickEventHandler(object sender, NiTaskDialogExpandoButtonClickEventArgs e);
}
