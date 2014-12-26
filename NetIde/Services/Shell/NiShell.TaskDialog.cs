using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.Shell.TaskDialog;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.Shell
{
    partial class NiShell
    {
        public HResult CreateTaskDialog(out INiTaskDialog taskDialog)
        {
            taskDialog = null;

            try
            {
                taskDialog = new NiTaskDialog(this);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class NiTaskDialog : ServiceObject, INiTaskDialog
        {
            private readonly IServiceProvider _serviceProvider;
            private readonly TaskDialog.TaskDialog _taskDialog;
            private readonly NiConnectionPoint<INiTaskDialogNotify> _connectionPoint = new NiConnectionPoint<INiTaskDialogNotify>();
            private ResourceManager _resourceManager;
            private IResource _mainIcon;
            private IResource _footerIcon;
            private bool _disposed;

            public ResourceManager ResourceManager
            {
                get
                {
                    if (_resourceManager == null)
                        _resourceManager = new ResourceManager();

                    return _resourceManager;
                }
            }

            public NiTaskDialog(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;

                _taskDialog = new TaskDialog.TaskDialog
                {
                    Callback = Callback
                };
            }

            private bool Callback(ActiveTaskDialog taskDialog, TaskDialogNotificationArgs args, object callbackData)
            {
                using (var niActiveTaskDialog = new NiActiveTaskDialog(this, taskDialog))
                {
                    switch (args.Notification)
                    {
                        case TaskDialogNotification.Created:
                            _connectionPoint.ForAll(p => p.OnCreated(niActiveTaskDialog));
                            return false;

                        case TaskDialogNotification.ButtonClicked:
                            bool close = false;
                            _connectionPoint.ForAll(p => p.OnButtonClick(niActiveTaskDialog, args.ButtonId, ref close));
                            return close;

                        case TaskDialogNotification.HyperlinkClicked:
                            _connectionPoint.ForAll(p => p.OnHyperlinkClicked(niActiveTaskDialog, args.Hyperlink));
                            return false;

                        case TaskDialogNotification.Timer:
                            bool resetTimer = false;
                            _connectionPoint.ForAll(p => p.OnTimer(niActiveTaskDialog, (int)args.TimerTickCount, ref resetTimer));
                            return resetTimer;

                        case TaskDialogNotification.Destroyed:
                            _connectionPoint.ForAll(p => p.OnDestroyed(niActiveTaskDialog));
                            return false;

                        case TaskDialogNotification.RadioButtonClicked:
                            _connectionPoint.ForAll(p => p.OnRadioButtonClicked(niActiveTaskDialog, args.ButtonId));
                            return false;

                        case TaskDialogNotification.DialogConstructed:
                            _connectionPoint.ForAll(p => p.OnDialogConstructed(niActiveTaskDialog));
                            return false;

                        case TaskDialogNotification.VerificationClicked:
                            _connectionPoint.ForAll(p => p.OnVerificationClicked(niActiveTaskDialog, args.VerificationFlagChecked));
                            return false;

                        case TaskDialogNotification.Help:
                            _connectionPoint.ForAll(p => p.OnHelp(niActiveTaskDialog));
                            return false;

                        case TaskDialogNotification.ExpandoButtonClicked:
                            _connectionPoint.ForAll(p => p.OnExpandoButtonClicked(niActiveTaskDialog, args.Expanded));
                            return false;

                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            public HResult Advise(object sink, out int cookie)
            {
                return _connectionPoint.Advise(sink, out cookie);
            }

            public HResult Advise(INiTaskDialogNotify sink, out int cookie)
            {
                return Advise((object)sink, out cookie);
            }

            public HResult Unadvise(int cookie)
            {
                return _connectionPoint.Unadvise(cookie);
            }

            public HResult Show(IntPtr owner, out bool verificationFlagChecked, out int radioButtonResult, out int result)
            {
                verificationFlagChecked = false;
                radioButtonResult = 0;
                result = 0;

                try
                {
                    if (owner == IntPtr.Zero)
                        owner = NativeMethods.GetActiveWindow();

                    if (String.IsNullOrEmpty(_taskDialog.MainInstruction))
                        _taskDialog.MainInstruction = ((INiEnv)_serviceProvider.GetService(typeof(INiEnv))).ContextName;

                    result = _taskDialog.Show(owner, out verificationFlagChecked, out radioButtonResult);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetWindowTitle(out string windowTitle)
            {
                windowTitle = null;

                try
                {
                    windowTitle = _taskDialog.WindowTitle;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetWindowTitle(string windowTitle)
            {
                try
                {
                    _taskDialog.WindowTitle = windowTitle;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetMainInstruction(out string mainInstruction)
            {
                mainInstruction = null;

                try
                {
                    mainInstruction = _taskDialog.MainInstruction;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetMainInstruction(string mainInstruction)
            {
                try
                {
                    _taskDialog.MainInstruction = mainInstruction;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetContent(out string content)
            {
                content = null;

                try
                {
                    content = _taskDialog.Content;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetContent(string content)
            {
                try
                {
                    _taskDialog.Content = content;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetVerificationText(out string verificationText)
            {
                verificationText = null;

                try
                {
                    verificationText = _taskDialog.VerificationText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetVerificationText(string verificationText)
            {
                try
                {
                    _taskDialog.VerificationText = verificationText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetExpandedInformation(out string expandedInformation)
            {
                expandedInformation = null;

                try
                {
                    expandedInformation = _taskDialog.ExpandedInformation;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetExpandedInformation(string expandedInformation)
            {
                try
                {
                    _taskDialog.ExpandedInformation = expandedInformation;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetExpandedControlText(out string expandedControlText)
            {
                expandedControlText = null;

                try
                {
                    expandedControlText = _taskDialog.ExpandedControlText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetExpandedControlText(string expandedControlText)
            {
                try
                {
                    _taskDialog.ExpandedControlText = expandedControlText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetCollapsedControlText(out string collapsedControlText)
            {
                collapsedControlText = null;

                try
                {
                    collapsedControlText = _taskDialog.CollapsedControlText;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetCollapsedControlText(string collapsedControlText)
            {
                try
                {
                    _taskDialog.CollapsedControlText = collapsedControlText;

                    return HResult.OK;

                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetFooter(out string footer)
            {
                footer = null;

                try
                {
                    footer = _taskDialog.Footer;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetFooter(string footer)
            {
                try
                {
                    _taskDialog.Footer = footer;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetCommonButtons(out NiTaskDialogCommonButtons commonButtons)
            {
                commonButtons = 0;

                try
                {
                    commonButtons = TaskDialogUtil.DecodeCommonButtons(_taskDialog.CommonButtons);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetCommonButtons(NiTaskDialogCommonButtons commonButtons)
            {
                try
                {
                    _taskDialog.CommonButtons = TaskDialogUtil.EncodeCommonButtons(commonButtons);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetMainIcon(out NiTaskDialogIcon mainIcon)
            {
                mainIcon = 0;

                try
                {
                    mainIcon = TaskDialogUtil.DecodeIcon(_taskDialog.MainIcon);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetMainIcon(NiTaskDialogIcon mainIcon)
            {
                try
                {
                    _taskDialog.MainIcon = TaskDialogUtil.EncodeIcon(mainIcon);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetMainIcon(out IResource mainIcon)
            {
                mainIcon = null;

                try
                {
                    mainIcon = _mainIcon;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetMainIcon(IResource mainIcon)
            {
                try
                {
                    _mainIcon = mainIcon;

                    _taskDialog.CustomMainIcon = _mainIcon != null
                        ? ResourceManager.GetIcon(_mainIcon)
                        : null;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetFooterIcon(out NiTaskDialogIcon footerIcon)
            {
                footerIcon = 0;

                try
                {
                    footerIcon = TaskDialogUtil.DecodeIcon(_taskDialog.FooterIcon);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetFooterIcon(NiTaskDialogIcon footerIcon)
            {
                try
                {
                    _taskDialog.FooterIcon = TaskDialogUtil.EncodeIcon(footerIcon);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetFooterIcon(out IResource footerIcon)
            {
                footerIcon = null;

                try
                {
                    footerIcon = _footerIcon;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetFooterIcon(IResource footerIcon)
            {
                try
                {
                    _footerIcon = footerIcon;

                    _taskDialog.CustomFooterIcon = _footerIcon != null
                        ? ResourceManager.GetIcon(_footerIcon)
                        : null;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetFlags(out NiTaskDialogFlags flags)
            {
                flags = 0;

                try
                {
                    if (_taskDialog.EnableHyperlinks)
                        flags |= NiTaskDialogFlags.EnableHyperlinks;
                    if (_taskDialog.AllowDialogCancellation)
                        flags |= NiTaskDialogFlags.AllowDialogCancellation;
                    if (_taskDialog.UseCommandLinks)
                        flags |= NiTaskDialogFlags.UseCommandLinks;
                    if (_taskDialog.UseCommandLinksNoIcon)
                        flags |= NiTaskDialogFlags.UseCommandLinksNoIcon;
                    if (_taskDialog.ExpandFooterArea)
                        flags |= NiTaskDialogFlags.ExpandFooterArea;
                    if (_taskDialog.ExpandedByDefault)
                        flags |= NiTaskDialogFlags.ExpandedByDefault;
                    if (_taskDialog.VerificationFlagChecked)
                        flags |= NiTaskDialogFlags.VerificationFlagChecked;
                    if (_taskDialog.ShowProgressBar)
                        flags |= NiTaskDialogFlags.ShowProgressBar;
                    if (_taskDialog.ShowMarqueeProgressBar)
                        flags |= NiTaskDialogFlags.ShowMarqueeProgressBar;
                    if (_taskDialog.CallbackTimer)
                        flags |= NiTaskDialogFlags.CallbackTimer;
                    if (_taskDialog.PositionRelativeToWindow)
                        flags |= NiTaskDialogFlags.PositionRelativeToWindow;
                    if (_taskDialog.RightToLeftLayout)
                        flags |= NiTaskDialogFlags.RightToLeftLayout;
                    if (_taskDialog.NoDefaultRadioButton)
                        flags |= NiTaskDialogFlags.NoDefaultRadioButton;
                    if (_taskDialog.CanBeMinimized)
                        flags |= NiTaskDialogFlags.CanBeMinimized;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetFlags(NiTaskDialogFlags flags)
            {
                try
                {
                    _taskDialog.EnableHyperlinks = flags.HasFlag(NiTaskDialogFlags.EnableHyperlinks);
                    _taskDialog.AllowDialogCancellation = flags.HasFlag(NiTaskDialogFlags.AllowDialogCancellation);
                    _taskDialog.UseCommandLinks = flags.HasFlag(NiTaskDialogFlags.UseCommandLinks);
                    _taskDialog.UseCommandLinksNoIcon = flags.HasFlag(NiTaskDialogFlags.UseCommandLinksNoIcon);
                    _taskDialog.ExpandFooterArea = flags.HasFlag(NiTaskDialogFlags.ExpandFooterArea);
                    _taskDialog.ExpandedByDefault = flags.HasFlag(NiTaskDialogFlags.ExpandedByDefault);
                    _taskDialog.VerificationFlagChecked = flags.HasFlag(NiTaskDialogFlags.VerificationFlagChecked);
                    _taskDialog.ShowProgressBar = flags.HasFlag(NiTaskDialogFlags.ShowProgressBar);
                    _taskDialog.ShowMarqueeProgressBar = flags.HasFlag(NiTaskDialogFlags.ShowMarqueeProgressBar);
                    _taskDialog.CallbackTimer = flags.HasFlag(NiTaskDialogFlags.CallbackTimer);
                    _taskDialog.PositionRelativeToWindow = flags.HasFlag(NiTaskDialogFlags.PositionRelativeToWindow);
                    _taskDialog.RightToLeftLayout = flags.HasFlag(NiTaskDialogFlags.RightToLeftLayout);
                    _taskDialog.NoDefaultRadioButton = flags.HasFlag(NiTaskDialogFlags.NoDefaultRadioButton);
                    _taskDialog.CanBeMinimized = flags.HasFlag(NiTaskDialogFlags.CanBeMinimized);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetDefaultButton(out int id)
            {
                id = 0;

                try
                {
                    id = _taskDialog.DefaultButton;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetDefaultButton(int id)
            {
                try
                {
                    _taskDialog.DefaultButton = id;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetDefaultRadioButton(out int id)
            {
                id = 0;

                try
                {
                    id = _taskDialog.DefaultRadioButton;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetDefaultRadioButton(int id)
            {
                try
                {
                    _taskDialog.DefaultRadioButton = id;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult GetWidth(out int width)
            {
                width = 0;

                try
                {
                    width = (int)_taskDialog.Width;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetWidth(int width)
            {
                try
                {
                    _taskDialog.Width = (uint)width;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult AddButton(int id, string text)
            {
                try
                {
                    _taskDialog.Buttons.Add(id, text);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult AddRadioButton(int id, string text)
            {
                try
                {
                    _taskDialog.RadioButtons.Add(id, text);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (!_disposed && disposing)
                {
                    if (_resourceManager != null)
                    {
                        _resourceManager.Dispose();
                        _resourceManager = null;
                    }

                    _disposed = true;
                }

                base.Dispose(disposing);
            }
        }

        private class NiActiveTaskDialog : ServiceObject, INiActiveTaskDialog
        {
            private readonly NiTaskDialog _taskDialog;
            private readonly ActiveTaskDialog _active;

            public NiActiveTaskDialog(NiTaskDialog taskDialog, ActiveTaskDialog active)
            {
                _taskDialog = taskDialog;
                _active = active;
            }

            public HResult GetHandle(out IntPtr handle)
            {
                handle = IntPtr.Zero;

                try
                {
                    handle = _active.Handle;

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult ClickButton(int id)
            {
                try
                {
                    _active.ClickButton(id);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult ClickRadioButton(int id)
            {
                try
                {
                    _active.ClickRadioButton(id);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult ClickVerification(bool @checked, bool setFocus)
            {
                try
                {
                    _active.ClickVerification(@checked, setFocus);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetButtonEnabled(int id, bool enabled)
            {
                try
                {
                    _active.EnableButton(id, enabled);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetRadioButtonEnabled(int id, bool enabled)
            {
                try
                {
                    _active.EnableRadioButton(id, enabled);
                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetMarqueeProgressBar(bool marquee)
            {
                try
                {
                    _active.SetMarqueeProgressBar(marquee);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProgressBarState(NiProgressBarState state)
            {
                try
                {
                    _active.SetProgressBarState(TaskDialogUtil.DecodeProgressBarState(state));

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProgressBarRange(int min, int max)
            {
                try
                {
                    _active.SetProgressBarRange((short)min, (short)max);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProgressBarPosition(int position)
            {
                try
                {
                    _active.SetProgressBarPosition(position);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetProgressBarMarquee(bool startMarquee, int speed)
            {
                try
                {
                    _active.SetProgressBarMarquee(startMarquee, (uint)speed);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetContent(string content)
            {
                try
                {
                    _active.SetContent(content);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateContent(string content)
            {
                try
                {
                    _active.UpdateContent(content);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetExpandedInformation(string expandedInformation)
            {
                try
                {
                    _active.SetExpandedInformation(expandedInformation);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateExpandedInformation(string expandedInformation)
            {
                try
                {
                    _active.UpdateExpandedInformation(expandedInformation);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetFooter(string footer)
            {
                try
                {
                    _active.SetFooter(footer);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateFooter(string footer)
            {
                try
                {
                    _active.UpdateFooter(footer);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetMainInstruction(string mainInstruction)
            {
                try
                {
                    _active.SetMainInstruction(mainInstruction);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateMainInstruction(string mainInstruction)
            {
                try
                {
                    _active.UpdateMainInstruction(mainInstruction);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult SetButtonElevationRequiredState(int id, bool elevationRequired)
            {
                try
                {
                    _active.SetButtonElevationRequiredState(id, elevationRequired);

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateMainIcon(NiTaskDialogIcon icon)
            {
                try
                {
                    _active.UpdateMainIcon(TaskDialogUtil.EncodeIcon(icon));

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateMainIcon(IResource icon)
            {
                try
                {
                    _active.UpdateMainIcon(icon == null ? null : _taskDialog.ResourceManager.GetIcon(icon));

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateFooterIcon(NiTaskDialogIcon icon)
            {
                try
                {
                    _active.UpdateFooterIcon(TaskDialogUtil.EncodeIcon(icon));

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }

            public HResult UpdateFooterIcon(IResource icon)
            {
                try
                {
                    _active.UpdateFooterIcon(icon == null ? null : _taskDialog.ResourceManager.GetIcon(icon));

                    return HResult.OK;
                }
                catch (Exception ex)
                {
                    return ErrorUtil.GetHResult(ex);
                }
            }
        }

        private static class TaskDialogUtil
        {
            public static TaskDialogCommonButtons EncodeCommonButtons(NiTaskDialogCommonButtons value)
            {
                TaskDialogCommonButtons result = 0;

                if (value.HasFlag(NiTaskDialogCommonButtons.OK))
                    result |= TaskDialogCommonButtons.OK;
                if (value.HasFlag(NiTaskDialogCommonButtons.Yes))
                    result |= TaskDialogCommonButtons.Yes;
                if (value.HasFlag(NiTaskDialogCommonButtons.No))
                    result |= TaskDialogCommonButtons.No;
                if (value.HasFlag(NiTaskDialogCommonButtons.Cancel))
                    result |= TaskDialogCommonButtons.Cancel;
                if (value.HasFlag(NiTaskDialogCommonButtons.Retry))
                    result |= TaskDialogCommonButtons.Retry;
                if (value.HasFlag(NiTaskDialogCommonButtons.Close))
                    result |= TaskDialogCommonButtons.Close;

                return result;
            }

            public static NiTaskDialogCommonButtons DecodeCommonButtons(TaskDialogCommonButtons value)
            {
                NiTaskDialogCommonButtons result = 0;

                if (value.HasFlag(TaskDialogCommonButtons.OK))
                    result |= NiTaskDialogCommonButtons.OK;
                if (value.HasFlag(TaskDialogCommonButtons.Yes))
                    result |= NiTaskDialogCommonButtons.Yes;
                if (value.HasFlag(TaskDialogCommonButtons.No))
                    result |= NiTaskDialogCommonButtons.No;
                if (value.HasFlag(TaskDialogCommonButtons.Cancel))
                    result |= NiTaskDialogCommonButtons.Cancel;
                if (value.HasFlag(TaskDialogCommonButtons.Retry))
                    result |= NiTaskDialogCommonButtons.Retry;
                if (value.HasFlag(TaskDialogCommonButtons.Close))
                    result |= NiTaskDialogCommonButtons.Close;

                return result;
            }

            public static TaskDialogIcon EncodeIcon(NiTaskDialogIcon value)
            {
                switch (value)
                {
                    case NiTaskDialogIcon.None:
                        return TaskDialogIcon.None;
                    case NiTaskDialogIcon.Warning:
                        return TaskDialogIcon.Warning;
                    case NiTaskDialogIcon.Error:
                        return TaskDialogIcon.Error;
                    case NiTaskDialogIcon.Information:
                        return TaskDialogIcon.Information;
                    case NiTaskDialogIcon.Shield:
                        return TaskDialogIcon.Shield;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }

            public static NiTaskDialogIcon DecodeIcon(TaskDialogIcon value)
            {
                switch (value)
                {
                    case TaskDialogIcon.None:
                        return NiTaskDialogIcon.None;
                    case TaskDialogIcon.Warning:
                        return NiTaskDialogIcon.Warning;
                    case TaskDialogIcon.Error:
                        return NiTaskDialogIcon.Error;
                    case TaskDialogIcon.Information:
                        return NiTaskDialogIcon.Information;
                    case TaskDialogIcon.Shield:
                        return NiTaskDialogIcon.Shield;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }

            public static ProgressBarState DecodeProgressBarState(NiProgressBarState value)
            {
                switch (value)
                {
                    case NiProgressBarState.Normal:
                        return ProgressBarState.Normal;
                    case NiProgressBarState.Error:
                        return ProgressBarState.Error;
                    case NiProgressBarState.Paused:
                        return ProgressBarState.Paused;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }
    }
}
