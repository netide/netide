using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialogVerificationFlagCheckedChangedEventArgs : NiTaskDialogEventArgs
    {
        public bool Checked { get; private set; }

        public NiTaskDialogVerificationFlagCheckedChangedEventArgs(INiActiveTaskDialog activeTaskDialog, bool @checked)
            : base(activeTaskDialog)
        {
            Checked = @checked;
        }
    }

    public delegate void NiTaskDialogVerificationFlagCheckedChangedEventHandler(object sender, NiTaskDialogVerificationFlagCheckedChangedEventArgs e);
}
