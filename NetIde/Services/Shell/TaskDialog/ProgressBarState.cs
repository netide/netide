using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Services.Shell.TaskDialog
{
    internal enum ProgressBarState
    {
        Normal = NativeMethods.PBST_NORMAL,
        Error = NativeMethods.PBST_ERROR,
        Paused = NativeMethods.PBST_PAUSED
    }
}