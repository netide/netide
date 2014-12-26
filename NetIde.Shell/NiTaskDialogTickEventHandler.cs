using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogTickEventArgs : NiTaskDialogEventArgs
    {
        public int Elapsed { get; private set; }

        public bool ResetTimer { get; set; }

        public NiTaskDialogTickEventArgs(INiActiveTaskDialog activeTaskDialog, int elapsed)
            : base(activeTaskDialog)
        {
            Elapsed = elapsed;
        }
    }

    public delegate void NiTaskDialogTickEventHandler(object sender, NiTaskDialogTickEventArgs e);
}
