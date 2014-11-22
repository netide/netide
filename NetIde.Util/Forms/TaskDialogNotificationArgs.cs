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

namespace NetIde.Util.Forms
{
    public class TaskDialogNotificationArgs
    {
        public TaskDialogNotification Notification { get; set; }

        public int ButtonId { get; set; }

        public string Hyperlink { get; set; }

        [CLSCompliant(false)]
        public uint TimerTickCount { get; set; }

        public bool VerificationFlagChecked { get; set; }

        public bool Expanded { get; set; }
    }
}
