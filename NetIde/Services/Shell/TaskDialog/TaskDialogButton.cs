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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace NetIde.Services.Shell.TaskDialog
{
    [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")] // Would be unused code as not required for usage.
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    internal struct TaskDialogButton
    {
        private int _buttonId;
        [MarshalAs(UnmanagedType.LPWStr)]
        private string _buttonText;

        public TaskDialogButton(int id, string text)
        {
            _buttonId = id;
            _buttonText = text;
        }

        public int ButtonId
        {
            get { return _buttonId; }
            set { _buttonId = value; }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set { _buttonText = value; }
        }
    }
}