using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    partial class TaskDialog
    {
        public static TaskDialogCommonButtons Show(string text, string caption, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        public static TaskDialogCommonButtons Show(string text, string caption, TaskDialogCommonButtons buttons)
        {
            return Show(null, text, caption, buttons, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(string text, string caption)
        {
            return Show(null, text, caption, TaskDialogCommonButtons.OK, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(string text)
        {
            return Show(null, text, null, TaskDialogCommonButtons.OK, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(IWin32Window owner, string text, string caption, TaskDialogCommonButtons buttons)
        {
            return Show(owner, text, caption, buttons, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, TaskDialogCommonButtons.OK, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(IWin32Window owner, string text)
        {
            return Show(owner, text, null, TaskDialogCommonButtons.OK, TaskDialogIcon.None);
        }

        public static TaskDialogCommonButtons Show(IWin32Window owner, string text, string caption, TaskDialogCommonButtons buttons, TaskDialogIcon icon)
        {
            return (TaskDialogCommonButtons)Show(owner, new TaskDialog
            {
                AllowDialogCancellation = (buttons & TaskDialogCommonButtons.Cancel) != 0,
                WindowTitle = caption,
                MainInstruction = text,
                CommonButtons = buttons,
                MainIcon = icon,
                PositionRelativeToWindow = true
            });
        }

        public static int Show(TaskDialog taskDialog)
        {
            bool verificationFlagChecked;

            return Show(null, out verificationFlagChecked, taskDialog);
        }

        public static int Show(IWin32Window owner, TaskDialog taskDialog)
        {
            if (taskDialog == null)
                throw new ArgumentNullException("taskDialog");

            bool verificationFlagChecked;

            return Show(owner, out verificationFlagChecked, taskDialog);
        }

        public static int Show(out bool verificationFlagChecked, TaskDialog taskDialog)
        {
            return Show(null, out verificationFlagChecked, taskDialog);
        }

        public static int Show(IWin32Window owner, out bool verificationFlagChecked, TaskDialog taskDialog)
        {
            if (taskDialog == null)
                throw new ArgumentNullException("taskDialog");

            return taskDialog.Show(owner, out verificationFlagChecked);
        }

        public static void Alert(string text)
        {
            Alert(null, text, null);
        }

        public static void Alert(string text, string caption)
        {
            Alert(null, text, caption);
        }

        public static void Alert(string text, string caption, TaskDialogIcon icon)
        {
            Alert(null, text, caption, icon);
        }

        public static void Alert(IWin32Window owner, string text)
        {
            Alert(owner, text, null);
        }

        public static void Alert(IWin32Window owner, string text, string caption)
        {
            Alert(owner, text, caption, TaskDialogIcon.Error);
        }

        public static void Alert(IWin32Window owner, string text, string caption, TaskDialogIcon icon)
        {
            Show(owner, text, caption, TaskDialogCommonButtons.OK, icon);
        }

        public static DialogResult Confirm(IWin32Window owner)
        {
            return Confirm(owner, Labels.AreYouSure);
        }

        public static DialogResult Confirm(IWin32Window owner, string message)
        {
            return Confirm(owner, message, TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No);
        }

        public static DialogResult Confirm(IWin32Window owner, string message, TaskDialogCommonButtons buttons)
        {
            return (DialogResult)Show(owner, new TaskDialog
            {
                MainInstruction = message,
                MainIcon = TaskDialogIcon.Warning,
                CommonButtons = buttons
            });
        }
    }
}
