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
    internal enum TaskDialogNotification
    {
        Created = 0,
        // Navigated = 1,
        ButtonClicked = 2, // wParam = Button ID
        HyperlinkClicked = 3, // lParam = (LPCWSTR)pszHREF
        Timer = 4, // wParam = Milliseconds since dialog created or timer reset
        Destroyed = 5,
        RadioButtonClicked = 6, // wParam = Radio Button ID
        DialogConstructed = 7,
        VerificationClicked = 8, // wParam = 1 if checkbox checked, 0 if not, lParam is unused and always 0
        Help = 9,
        ExpandoButtonClicked = 10 // wParam = 0 (dialog is now collapsed), wParam != 0 (dialog is now expanded)
    }
}