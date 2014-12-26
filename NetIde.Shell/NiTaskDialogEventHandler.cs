using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogEventArgs : EventArgs
    {
        public INiActiveTaskDialog ActiveTaskDialog { get; private set; }

        public NiTaskDialogEventArgs(INiActiveTaskDialog activeTaskDialog)
        {
            if (activeTaskDialog == null)
                throw new ArgumentNullException("activeTaskDialog");

            ActiveTaskDialog = activeTaskDialog;
        }
    }

    public delegate void NiTaskDialogEventHandler(object sender, NiTaskDialogEventArgs e);
}
