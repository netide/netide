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
    internal enum TaskDialogIcon
    {
        None = 0,
        Warning = 0xFFFF, // MAKEINTRESOURCEW(-1)
        Error = 0xFFFE, // MAKEINTRESOURCEW(-2)
        Information = 0xFFFD, // MAKEINTRESOURCEW(-3)
        Shield = 0xFFFC, // MAKEINTRESOURCEW(-4)
    }
}