using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiTaskDialogCommonButtons
    {
        None = 0,
        OK = 1 << 0,
        Yes = 1 << 1,
        No = 1 << 2,
        Cancel = 1 << 3,
        Retry = 1 << 4,
        Close = 1 << 5,
        OKCancel = OK | Cancel,
        YesNoCancel = Yes | No | Cancel,
        YesNo = Yes | No,
        RetryCancel = Retry | Cancel
    }
}
