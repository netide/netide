using System;
using System.Collections.Generic;
using System.Text;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public enum ProgressBarState
    {
        Normal = NativeMethods.PBST_NORMAL,
        Error = NativeMethods.PBST_ERROR,
        Paused = NativeMethods.PBST_PAUSED
    }
}