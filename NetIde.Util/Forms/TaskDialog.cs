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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Util.Win32;

namespace NetIde.Util.Forms
{
    public partial class TaskDialog
    {
        public static readonly Version RequiredOSVersion = new Version(6, 0, 5243);

        static TaskDialog()
        {
            DefaultWindowTitle = AssemblyInformation.GetProductTitle(AssemblyInformation.FindEntryAssembly());
        }

        public static bool ForceEmulation { get; set; }

        public static bool TranslateCommonButtons { get; set; }

        public static string DefaultWindowTitle { get; set; }

        private TaskDialogButton[] _buttons;
        private TaskDialogButton[] _radioButtons;
        private NativeMethods.TASKDIALOG_FLAGS _flags;
        private EmulateTaskDialog _emulateTaskDialog;

        public TaskDialog()
        {
            Reset();
        }

        public static bool IsAvailable
        {
            get
            {
                if (ForceEmulation)
                    return false;

                var os = Environment.OSVersion;

                if (os.Platform != PlatformID.Win32NT)
                    return false;

                return (os.Version.CompareTo(TaskDialog.RequiredOSVersion) >= 0);
            }
        }

        public string WindowTitle { get; set; }

        public string MainInstruction { get; set; }

        public string Content { get; set; }

        public TaskDialogCommonButtons CommonButtons { get; set; }

        public TaskDialogIcon MainIcon { get; set; }

        public Icon CustomMainIcon { get; set; }

        public TaskDialogIcon FooterIcon { get; set; }

        public Icon CustomFooterIcon { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
        public TaskDialogButton[] Buttons
        {
            get
            {
                return _buttons;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _buttons = value;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")] // Style of use is like single value. Array is of value types.
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")] // Returns a reference, not a copy.
        public TaskDialogButton[] RadioButtons
        {
            get
            {
                return _radioButtons;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _radioButtons = value;
            }
        }

        public bool EnableHyperlinks
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_ENABLE_HYPERLINKS, value); }
        }

        public bool AllowDialogCancellation
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_ALLOW_DIALOG_CANCELLATION, value); }
        }

        public bool UseCommandLinks
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS, value); }
        }

        public bool UseCommandLinksNoIcon
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_USE_COMMAND_LINKS_NO_ICON, value); }
        }

        public bool ExpandFooterArea
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_EXPAND_FOOTER_AREA, value); }
        }

        public bool ExpandedByDefault
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_EXPANDED_BY_DEFAULT, value); }
        }

        public bool VerificationFlagChecked
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_VERIFICATION_FLAG_CHECKED, value); }
        }

        public bool ShowProgressBar
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_PROGRESS_BAR, value); }
        }

        public bool ShowMarqueeProgressBar
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_SHOW_MARQUEE_PROGRESS_BAR, value); }
        }

        public bool CallbackTimer
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_CALLBACK_TIMER, value); }
        }

        public bool PositionRelativeToWindow
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_POSITION_RELATIVE_TO_WINDOW, value); }
        }

        public bool RightToLeftLayout
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_RTL_LAYOUT, value); }
        }

        public bool NoDefaultRadioButton
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_NO_DEFAULT_RADIO_BUTTON, value); }
        }

        public bool CanBeMinimized
        {
            get { return (_flags & NativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED) != 0; }
            set { SetFlag(NativeMethods.TASKDIALOG_FLAGS.TDF_CAN_BE_MINIMIZED, value); }
        }

        public int DefaultButton { get; set; }

        public int DefaultRadioButton { get; set; }

        public string VerificationText { get; set; }

        public string ExpandedInformation { get; set; }

        public string ExpandedControlText { get; set; }

        public string CollapsedControlText { get; set; }

        public string Footer { get; set; }

        [CLSCompliant(false)]
        public uint Width { get; set; }

        public TaskDialogCallback Callback { get; set; }

        public object CallbackData { get; set; }

        public void Reset()
        {
            WindowTitle = null;
            MainInstruction = null;
            Content = null;
            CommonButtons = 0;
            MainIcon = TaskDialogIcon.None;
            CustomMainIcon = null;
            FooterIcon = TaskDialogIcon.None;
            CustomFooterIcon = null;
            _buttons = new TaskDialogButton[0];
            _radioButtons = new TaskDialogButton[0];
            _flags = 0;
            DefaultButton = 0;
            DefaultRadioButton = 0;
            VerificationText = null;
            ExpandedInformation = null;
            ExpandedControlText = null;
            CollapsedControlText = null;
            Footer = null;
            Callback = null;
            CallbackData = null;
            Width = 0;
        }

        public int Show()
        {
            bool verificationFlagChecked;
            int radioButtonResult;
            return Show(IntPtr.Zero, out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IWin32Window owner)
        {
            bool verificationFlagChecked;
            int radioButtonResult;
            return Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IntPtr hwndOwner)
        {
            bool verificationFlagChecked;
            int radioButtonResult;
            return Show(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IWin32Window owner, out bool verificationFlagChecked)
        {
            int radioButtonResult;
            return Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IntPtr hwndOwner, out bool verificationFlagChecked)
        {
            // We have to call a private version or PreSharp gets upset about a unsafe
            // block in a public method. (PreSharp error 56505)
            int radioButtonResult;
            return PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IWin32Window owner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            return Show((owner == null ? IntPtr.Zero : owner.Handle), out verificationFlagChecked, out radioButtonResult);
        }

        public int Show(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            // We have to call a private version or PreSharp gets upset about a unsafe
            // block in a public method. (PreSharp error 56505)
            return PrivateShow(hwndOwner, out verificationFlagChecked, out radioButtonResult);
        }

        public void Close()
        {
            if (IsAvailable)
                throw new InvalidOperationException("Cannot close native task dialog");

            if (_emulateTaskDialog != null)
                _emulateTaskDialog.Dispose();
        }

        private int PrivateShow(IntPtr hwndOwner, out bool verificationFlagChecked, out int radioButtonResult)
        {
            if (TranslateCommonButtons)
                PerformTranslate();

            verificationFlagChecked = false;
            radioButtonResult = 0;
            int result;

            if (!TaskDialog.IsAvailable)
            {
                // Hand it off to emulator.
                _emulateTaskDialog = new EmulateTaskDialog(this);

                try
                {
                    if (hwndOwner == IntPtr.Zero)
                        _emulateTaskDialog.ShowDialog();
                    else
                        _emulateTaskDialog.ShowDialog(new WindowHandleWrapper(hwndOwner));

                    verificationFlagChecked = _emulateTaskDialog.TaskDialogVerificationFlagChecked;
                    radioButtonResult = _emulateTaskDialog.TaskDialogRadioButtonResult;
                    result = _emulateTaskDialog.TaskDialogResult;
                }
                finally
                {
                    _emulateTaskDialog.Dispose();
                    _emulateTaskDialog = null;
                }

                return result;
            }

            NativeMethods.TASKDIALOGCONFIG config = new NativeMethods.TASKDIALOGCONFIG();

            try
            {
                config.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.TASKDIALOGCONFIG));
                config.hwndParent = hwndOwner;
                config.dwFlags = _flags;
                config.dwCommonButtons = CommonButtons;

                if (!string.IsNullOrEmpty(WindowTitle))
                {
                    config.pszWindowTitle = WindowTitle;
                }
                else
                {
                    config.pszWindowTitle = TaskDialog.DefaultWindowTitle;
                }

                config.MainIcon = (IntPtr)MainIcon;
                if (CustomMainIcon != null)
                {
                    config.dwFlags |= NativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_MAIN;
                    config.MainIcon = CustomMainIcon.Handle;
                }

                if (!string.IsNullOrEmpty(MainInstruction))
                {
                    config.pszMainInstruction = MainInstruction;
                }

                if (!string.IsNullOrEmpty(Content))
                {
                    config.pszContent = Content;
                }

                TaskDialogButton[] customButtons = _buttons;
                if (customButtons.Length > 0)
                {
                    // Hand marshal the buttons array.
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    config.pButtons = Marshal.AllocHGlobal(elementSize * customButtons.Length);
                    for (int i = 0; i < customButtons.Length; i++)
                    {
                        Marshal.StructureToPtr(customButtons[i], (IntPtr)((long)config.pButtons + elementSize * i), false);

                        config.cButtons++;
                    }
                }

                TaskDialogButton[] customRadioButtons = _radioButtons;
                if (customRadioButtons.Length > 0)
                {
                    // Hand marshal the buttons array.
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    config.pRadioButtons = Marshal.AllocHGlobal(elementSize * customRadioButtons.Length);
                    for (int i = 0; i < customRadioButtons.Length; i++)
                    {
                        Marshal.StructureToPtr(customRadioButtons[i], (IntPtr)((long)config.pRadioButtons + (elementSize * i)), false);

                        config.cRadioButtons++;
                    }
                }

                config.nDefaultButton = DefaultButton;
                config.nDefaultRadioButton = DefaultRadioButton;

                if (!string.IsNullOrEmpty(VerificationText))
                    config.pszVerificationText = VerificationText;

                if (!string.IsNullOrEmpty(ExpandedInformation))
                    config.pszExpandedInformation = ExpandedInformation;

                if (!string.IsNullOrEmpty(ExpandedControlText))
                    config.pszExpandedControlText = ExpandedControlText;

                if (!string.IsNullOrEmpty(CollapsedControlText))
                    config.pszCollapsedControlText = CollapsedControlText;

                config.FooterIcon = (IntPtr)FooterIcon;
                if (CustomFooterIcon != null)
                {
                    config.dwFlags |= NativeMethods.TASKDIALOG_FLAGS.TDF_USE_HICON_FOOTER;
                    config.FooterIcon = CustomFooterIcon.Handle;
                }

                if (!string.IsNullOrEmpty(Footer))
                    config.pszFooter = Footer;

                // If our user has asked for a callback then we need to ask for one to
                // translate to the friendly version.
                if (Callback != null)
                    config.pfCallback = PrivateCallback;

                config.cxWidth = Width;

                using (new ActivationContext())
                {
                    // The call all this mucking about is here for.
                    NativeMethods.TaskDialogIndirect(ref config, out result, out radioButtonResult, out verificationFlagChecked);
                }
            }
            finally
            {
                // Free the unmanged memory needed for the button arrays.
                // There is the possiblity of leaking memory if the app-domain is destroyed in a non clean way
                // and the hosting OS process is kept alive but fixing this would require using hardening techniques
                // that are not required for the users of this class.
                if (config.pButtons != IntPtr.Zero)
                {
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    for (int i = 0; i < config.cButtons; i++)
                    {
                        Marshal.DestroyStructure((IntPtr)((long)config.pButtons + (elementSize * i)), typeof(TaskDialogButton));
                    }

                    Marshal.FreeHGlobal(config.pButtons);
                }

                if (config.pRadioButtons != IntPtr.Zero)
                {
                    int elementSize = Marshal.SizeOf(typeof(TaskDialogButton));
                    for (int i = 0; i < config.cRadioButtons; i++)
                    {
                        Marshal.DestroyStructure((IntPtr)((long)config.pRadioButtons + (elementSize * i)), typeof(TaskDialogButton));
                    }

                    Marshal.FreeHGlobal(config.pRadioButtons);
                }
            }

            return result;
        }

        private void PerformTranslate()
        {
            // We can't translate dialogs that have command link buttons since
            // we have no way of forcing these buttons into the footer.

            if (
                CommonButtons == TaskDialogCommonButtons.None ||
                (UseCommandLinks && Buttons.Length > 0)
            )
                return;

            var buttons = new List<TaskDialogButton>(Buttons);

            PerformTranslateButton(buttons, TaskDialogCommonButtons.OK, DialogResult.OK, Labels.Button_OK);
            PerformTranslateButton(buttons, TaskDialogCommonButtons.Yes, DialogResult.Yes, Labels.Button_Yes);
            PerformTranslateButton(buttons, TaskDialogCommonButtons.No, DialogResult.No, Labels.Button_No);
            PerformTranslateButton(buttons, TaskDialogCommonButtons.Retry, DialogResult.Retry, Labels.Button_Retry);
            PerformTranslateButton(buttons, TaskDialogCommonButtons.Close, DialogResult.OK, Labels.Button_Close);
            PerformTranslateButton(buttons, TaskDialogCommonButtons.Cancel, DialogResult.Cancel, Labels.Button_Cancel);

            Buttons = buttons.ToArray();

            CommonButtons = TaskDialogCommonButtons.None;
        }

        private void PerformTranslateButton(List<TaskDialogButton> buttons, TaskDialogCommonButtons button, DialogResult dialogResult, string label)
        {
            if ((CommonButtons & button) != 0)
            {
                buttons.Add(new TaskDialogButton
                {
                    ButtonId = (int)dialogResult,
                    ButtonText = label
                });
            }
        }

        private int PrivateCallback([In] IntPtr hwnd, [In] uint msg, [In] UIntPtr wparam, [In] IntPtr lparam, [In] IntPtr refData)
        {
            TaskDialogCallback callback = Callback;
            if (callback != null)
            {
                // Prepare arguments for the callback to the user we are insulating from Interop casting sillyness.

                // Future: Consider reusing a single ActiveTaskDialog object and mark it as destroyed on the destry notification.
                ActiveTaskDialog activeDialog = new ActiveTaskDialog(hwnd);
                TaskDialogNotificationArgs args = new TaskDialogNotificationArgs();
                args.Notification = (TaskDialogNotification)msg;
                switch (args.Notification)
                {
                    case TaskDialogNotification.ButtonClicked:
                    case TaskDialogNotification.RadioButtonClicked:
                        args.ButtonId = (int)wparam;
                        break;
                    case TaskDialogNotification.HyperlinkClicked:
                        args.Hyperlink = Marshal.PtrToStringUni(lparam);
                        break;
                    case TaskDialogNotification.Timer:
                        args.TimerTickCount = (uint)wparam;
                        break;
                    case TaskDialogNotification.VerificationClicked:
                        args.VerificationFlagChecked = (wparam != UIntPtr.Zero);
                        break;
                    case TaskDialogNotification.ExpandoButtonClicked:
                        args.Expanded = (wparam != UIntPtr.Zero);
                        break;
                }

                return (callback(activeDialog, args, CallbackData) ? 1 : 0);
            }

            return 0; // false;
        }

        private void SetFlag(NativeMethods.TASKDIALOG_FLAGS flag, bool value)
        {
            if (value)
                _flags |= flag;
            else
                _flags &= ~flag;
        }

        private class WindowHandleWrapper : IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public WindowHandleWrapper(IntPtr handle)
            {
                Handle = handle;
            }
        }
    }

    public delegate bool TaskDialogCallback(ActiveTaskDialog taskDialog, TaskDialogNotificationArgs args, object callbackData);
}
