/*
 * Copyright © 2007 KevinGre
 * 
 * Design Notes:-
 * --------------
 * References:
 * - http://www.codeproject.com/KB/vista/TaskDialogWinForms.aspx
 * 
 * Revision Control:-
 * ------------------
 * Created On: 2007 January 02
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Services.Shell.TaskDialog
{
    [Flags]
    internal enum TaskDialogCommonButtons
    {
        None = 0,
        OK = 0x0001,
        Yes = 0x0002,
        No = 0x0004,
        Cancel = 0x0008,
        Retry = 0x0010,
        Close = 0x0020,
        OKCancel = OK | Cancel,
        YesNoCancel = Yes | No | Cancel,
        YesNo = Yes | No,
        RetryCancel = Retry | Cancel
    }
}