using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTaskDialog : Component
    {
        private readonly Dictionary<int, NiTaskDialogButton> _buttons = new Dictionary<int, NiTaskDialogButton>();
        private readonly Formattable _windowTitle = new Formattable();
        private readonly Formattable _mainInstruction = new Formattable();
        private readonly Formattable _content = new Formattable();
        private readonly Formattable _verificationText = new Formattable();
        private readonly Formattable _expandedInformation = new Formattable();
        private readonly Formattable _expandedControlText = new Formattable();
        private readonly Formattable _collapsedControlText = new Formattable();
        private readonly Formattable _footerText = new Formattable();

        [Description("Text shown in the title bar")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string WindowTitle
        {
            get { return _windowTitle.Text; }
            set { _windowTitle.Text = value; }
        }

        [Description("Main instruction message")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string MainInstruction
        {
            get { return _mainInstruction.Text; }
            set { _mainInstruction.Text = value; }
        }

        [Description("Descriptive message")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        [Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        public string Content
        {
            get { return _content.Text; }
            set { _content.Text = value; }
        }

        [Description("Text of the verification check box")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string VerificationText
        {
            get { return _verificationText.Text; }
            set { _verificationText.Text = value; }
        }

        [Description("Text shown when the expanded information is expanded")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string ExpandedInformation
        {
            get { return _expandedInformation.Text; }
            set { _expandedInformation.Text = value; }
        }

        [Description("Text of the button that shows the expanded information")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string ExpandedControlText
        {
            get { return _expandedControlText.Text; }
            set { _expandedControlText.Text = value; }
        }

        [Description("Text of the button that hides the expanded information")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string CollapsedControlText
        {
            get { return _collapsedControlText.Text; }
            set { _collapsedControlText.Text = value; }
        }

        [Description("Footer text")]
        [Category("Appearance")]
        [Localizable(true)]
        [Bindable(true)]
        [DefaultValue("")]
        public string FooterText
        {
            get { return _footerText.Text; }
            set { _footerText.Text = value; }
        }

        [Description("Visible common buttons")]
        [Category("Behavior")]
        [DefaultValue((NiTaskDialogCommonButtons)0)]
        [Editor(typeof(NiTaskDialogCommonButtonsEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(NiTaskDialogCommonButtonsConverter))]
        public NiTaskDialogCommonButtons CommonButtons { get; set; }

        [Description("Main icon")]
        [Category("Appearance")]
        [DefaultValue((NiTaskDialogIcon)0)]
        public NiTaskDialogIcon MainIcon { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IResource CustomMainIcon { get; set; }

        [Description("Footer icon")]
        [Category("Appearance")]
        [DefaultValue((NiTaskDialogIcon)0)]
        public NiTaskDialogIcon FooterIcon { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public IResource CustomFooterIcon { get; set; }

        [Description("Automatically detect hyperlinks in the content")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool EnableHyperlinks { get; set; }

        [Description("Whether the task dialog can be cancelled")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CanCancel { get; set; }

        [Description("Style of the custom buttons")]
        [Category("Appearance")]
        [DefaultValue(NiTaskDialogButtonStyle.Button)]
        public NiTaskDialogButtonStyle ButtonStyle { get; set; }

        [Description("Location of the expanded information")]
        [Category("Appearance")]
        [DefaultValue(NiTaskDialogExpandedInformationLocation.BelowContent)]
        public NiTaskDialogExpandedInformationLocation ExpandedInformationLocation { get; set; }

        [Description("Whether the expanded information is visible")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ExpandedByDefault { get; set; }

        [Description("Whether the verification flag is checked when opening the task dialog")]
        [Category("Appearance")]
        [DefaultValue(false)]
        public bool VerificationFlagInitiallyChecked { get; set; }

        [Browsable(false)]
        public bool VerificationFlagChecked { get; private set; }

        [Description("Style of the progress bar")]
        [Category("Appearance")]
        [DefaultValue(NiTaskDialogProgressBarStyle.Hidden)]
        public NiTaskDialogProgressBarStyle ProgressBar { get; set; }

        [Description("Whether the callback timer is enabled")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CallbackTimer { get; set; }

        [Description("Whether to center the task dialog over the parent form")]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool CenterParent { get; set; }

        [Localizable(true)]
        [DefaultValue(RightToLeft.Inherit)]
        [Category("Appearance")]
        [Description("Layout the task dialog right to left")]
        public RightToLeft RightToLeft { get; set; }

        [Description("Visible common buttons")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool NoDefaultRadioButton { get; set; }

        [Description("Visible common buttons")]
        [Category("Behavior")]
        [DefaultValue(false)]
        public bool CanMinimize { get; set; }

        [Browsable(false)]
        public NiTaskDialogButton SelectedButton { get; private set; }

        [Browsable(false)]
        public NiTaskDialogButton SelectedRadioButton { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Category("Behavior")]
        [Description("Buttons")]
        [Localizable(true)]
        public NiTaskDialogButtonCollection Buttons { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [MergableProperty(false)]
        [Category("Behavior")]
        [Description("Radio buttons")]
        [Localizable(true)]
        public NiTaskDialogButtonCollection RadioButtons { get; private set; }

        [Description("Width of the task dialog")]
        [Category("Appearance")]
        [DefaultValue(0)]
        public int Width { get; set; }

        [DefaultValue(null)]
        [Category("Data")]
        [Description("Container control of the task dialog")]
        public ContainerControl ContainerControl { get; set; }

        public override ISite Site
        {
            set
            {
                base.Site = value;
                if (value == null)
                    return;

                var designerHost = value.GetService(typeof(IDesignerHost)) as IDesignerHost;
                if (designerHost == null)
                    return;

                var rootComponent = designerHost.RootComponent;
                if (!(rootComponent is ContainerControl))
                    return;

                ContainerControl = (ContainerControl)rootComponent;
            }
        }

        [Category("Action")]
        [Description("Occurs when a button is clicked")]
        public event NiTaskDialogButtonClickEventHandler ButtonClick;

        protected virtual void OnButtonClick(NiTaskDialogButtonClickEventArgs e)
        {
            var handler = ButtonClick;
            if (handler != null)
                handler(this, e);
        }

        [Category("Behavior")]
        [Description("Occurs when the task dialog has been created")]
        public event NiTaskDialogEventHandler Create;

        protected virtual void OnCreate(NiTaskDialogEventArgs e)
        {
            var handler = Create;
            if (handler != null)
                handler(this, e);
        }

        [Category("Behavior")]
        [Description("Occurs when the task dialog has been destroyed")]
        public event NiTaskDialogEventHandler Destroy;

        protected virtual void OnDestroy(NiTaskDialogEventArgs e)
        {
            var handler = Destroy;
            if (handler != null)
                handler(this, e);
        }

        [Category("Behavior")]
        [Description("Occurs when the physical task dialog has been constructed")]
        public event NiTaskDialogEventHandler CreateDialog;

        protected virtual void OnCreateDialog(NiTaskDialogEventArgs e)
        {
            var handler = CreateDialog;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when the expando buton has been clicked")]
        public event NiTaskDialogExpandoButtonClickEventHandler ExpandoButtonClick;

        protected virtual void OnExpandoButtonClick(NiTaskDialogExpandoButtonClickEventArgs e)
        {
            var handler = ExpandoButtonClick;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when help has been requested")]
        public event NiTaskDialogEventHandler Help;

        protected virtual void OnHelp(NiTaskDialogEventArgs e)
        {
            var handler = Help;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when a hyperlink has been clicked")]
        public event NiTaskDialogHyperlinkClickEventHandler HyperlinkClick;

        protected virtual void OnHyperlinkClick(NiTaskDialogHyperlinkClickEventArgs e)
        {
            var handler = HyperlinkClick;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when a radio button has been clicked")]
        public event NiTaskDialogButtonClickEventHandler RadioButtonClick;

        protected virtual void OnRadioButtonClick(NiTaskDialogButtonClickEventArgs e)
        {
            var handler = RadioButtonClick;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when the timer has elapsed")]
        public event NiTaskDialogTickEventHandler Tick;

        protected virtual void OnTick(NiTaskDialogTickEventArgs e)
        {
            var handler = Tick;
            if (handler != null)
                handler(this, e);
        }

        [Category("Action")]
        [Description("Occurs when the verification flag has been checked")]
        public event NiTaskDialogVerificationFlagCheckedChangedEventHandler VerificationFlagCheckedChanged;

        protected virtual void OnVerificationFlagCheckedChanged(NiTaskDialogVerificationFlagCheckedChangedEventArgs e)
        {
            var handler = VerificationFlagCheckedChanged;
            if (handler != null)
                handler(this, e);
        }

        public NiTaskDialog()
        {
            EnableHyperlinks = true;
            CenterParent = true;
            ButtonStyle = NiTaskDialogButtonStyle.Button;
            ExpandedInformationLocation = NiTaskDialogExpandedInformationLocation.BelowContent;
            RightToLeft = RightToLeft.Inherit;
            ProgressBar = NiTaskDialogProgressBarStyle.Hidden;
            Buttons = new NiTaskDialogButtonCollection();
            RadioButtons = new NiTaskDialogButtonCollection();
        }

        public void FormatWindowTitle(params object[] args)
        {
            _windowTitle.Format(args);
        }

        public void FormatMainInstruction(params object[] args)
        {
            _mainInstruction.Format(args);
        }

        public void FormatContent(params object[] args)
        {
            _content.Format(args);
        }

        public void FormatVerificationText(params object[] args)
        {
            _verificationText.Format(args);
        }

        public void FormatExpandedInformation(params object[] args)
        {
            _expandedInformation.Format(args);
        }

        public void FormatExpandedControlText(params object[] args)
        {
            _expandedControlText.Format(args);
        }

        public void FormatCollapsedControlText(params object[] args)
        {
            _collapsedControlText.Format(args);
        }

        public void FormatFooterText(params object[] args)
        {
            _footerText.Format(args);
        }

        public NiTaskDialogButton FindButton(int id)
        {
            NiTaskDialogButton button;
            _buttons.TryGetValue(id, out button);
            return button;
        }

        public DialogResult ShowDialog()
        {
            return ShowDialog(null, null);
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            return ShowDialog(owner, null);
        }

        public DialogResult ShowDialog(IWin32Window owner, IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                serviceProvider = ResolveSite();

                if (serviceProvider == null)
                    throw new InvalidOperationException("Cannot resolve site");
            }

            INiTaskDialog taskDialog;
            ErrorUtil.ThrowOnFailure(((INiShell)serviceProvider.GetService(typeof(INiShell))).CreateTaskDialog(
                out taskDialog
            ));

            NiTaskDialogFlags flags = 0;
            if (CanCancel)
                flags |= NiTaskDialogFlags.AllowDialogCancellation;
            if (CallbackTimer)
                flags |= NiTaskDialogFlags.CallbackTimer;
            if (CanMinimize)
                flags |= NiTaskDialogFlags.CanBeMinimized;
            if (EnableHyperlinks)
                flags |= NiTaskDialogFlags.EnableHyperlinks;
            if (ExpandedByDefault)
                flags |= NiTaskDialogFlags.ExpandedByDefault;
            if (NoDefaultRadioButton)
                flags |= NiTaskDialogFlags.NoDefaultRadioButton;
            if (CenterParent)
                flags |= NiTaskDialogFlags.PositionRelativeToWindow;
            if (VerificationFlagInitiallyChecked)
                flags |= NiTaskDialogFlags.VerificationFlagChecked;

            switch (RightToLeft)
            {
                case RightToLeft.Yes:
                    flags |= NiTaskDialogFlags.RightToLeftLayout;
                    break;

                case RightToLeft.Inherit:
                    if (ContainerControl != null && ContainerControl.RightToLeft == RightToLeft.Yes)
                        flags |= NiTaskDialogFlags.RightToLeftLayout;
                    break;
            }

            switch (ProgressBar)
            {
                case NiTaskDialogProgressBarStyle.Normal:
                    flags |= NiTaskDialogFlags.ShowProgressBar;
                    break;
                case NiTaskDialogProgressBarStyle.Marquee:
                    flags |= NiTaskDialogFlags.ShowProgressBar | NiTaskDialogFlags.ShowMarqueeProgressBar;
                    break;
            }

            switch (ButtonStyle)
            {
                case NiTaskDialogButtonStyle.CommandLink:
                    flags |= NiTaskDialogFlags.UseCommandLinks;
                    break;
                case NiTaskDialogButtonStyle.CommandLinkWithoutIcon:
                    flags |= NiTaskDialogFlags.UseCommandLinks | NiTaskDialogFlags.UseCommandLinksNoIcon;
                    break;
            }

            if (ExpandedInformationLocation == NiTaskDialogExpandedInformationLocation.Bottom)
                flags |= NiTaskDialogFlags.ExpandFooterArea;

            ErrorUtil.ThrowOnFailure(taskDialog.SetFlags(flags));

            ErrorUtil.ThrowOnFailure(taskDialog.SetCommonButtons(CommonButtons));

            if (MainIcon != 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetMainIcon(MainIcon));
            if (CustomMainIcon != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetMainIcon(CustomMainIcon));
            if (FooterIcon != 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetFooterIcon(FooterIcon));
            if (CustomFooterIcon != null)
                ErrorUtil.ThrowOnFailure(taskDialog.SetFooterIcon(CustomFooterIcon));

            _buttons.Clear();

            bool hadDefault = false;

            foreach (var button in Buttons)
            {
                var id = AddButton(_buttons, button);
                ErrorUtil.ThrowOnFailure(taskDialog.AddButton(
                    id,
                    button.FormattedText
                ));

                if (!hadDefault && button.Default)
                {
                    hadDefault = true;
                    ErrorUtil.ThrowOnFailure(taskDialog.SetDefaultButton(id));
                }
            }

            hadDefault = false;

            foreach (var button in RadioButtons)
            {
                var id = AddButton(_buttons, button);
                ErrorUtil.ThrowOnFailure(taskDialog.AddRadioButton(
                    id,
                    button.FormattedText
                ));

                if (!hadDefault && button.Default)
                {
                    hadDefault = true;
                    ErrorUtil.ThrowOnFailure(taskDialog.SetDefaultRadioButton(id));
                }
            }

            if (Width != 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetWidth(Width));

            string collapsedControlText = _collapsedControlText.Formatted;
            if (collapsedControlText.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetCollapsedControlText(collapsedControlText));
            string content = _content.Formatted;
            if (content.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetContent(content));
            string expandedControlText = _expandedControlText.Formatted;
            if (expandedControlText.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetExpandedControlText(expandedControlText));
            string expandedInformation = _expandedInformation.Formatted;
            if (expandedInformation.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetExpandedInformation(expandedInformation));
            string footerText = _footerText.Formatted;
            if (footerText.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetFooter(footerText));
            string mainInstruction = _mainInstruction.Formatted;
            if (mainInstruction.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetMainInstruction(mainInstruction));
            string verificationText = _verificationText.Formatted;
            if (verificationText.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetVerificationText(verificationText));
            string windowTitle = _windowTitle.Formatted;
            if (windowTitle.Length > 0)
                ErrorUtil.ThrowOnFailure(taskDialog.SetWindowTitle(windowTitle));

            new Notify(this, taskDialog);

            bool verificationFlagChecked;
            int radioButtonClicked;
            int result;
            ErrorUtil.ThrowOnFailure(taskDialog.Show(
                owner == null ? IntPtr.Zero : owner.Handle,
                out verificationFlagChecked,
                out radioButtonClicked,
                out result
            ));

            VerificationFlagChecked = verificationFlagChecked;

            SelectedButton = FindButton(result);
            SelectedRadioButton = FindButton(radioButtonClicked);

            if (SelectedButton != null)
                return SelectedButton.DialogResult;

            return (DialogResult)result;
        }

        private int AddButton(Dictionary<int, NiTaskDialogButton> buttons, NiTaskDialogButton button)
        {
            int id = 101 + buttons.Count;
            buttons.Add(id, button);
            return id;
        }

        private IServiceProvider ResolveSite()
        {
            if (Site != null)
                return Site;
            if (ContainerControl == null)
                return null;
            if (ContainerControl.Site != null)
                return ContainerControl.Site;
            var form = ContainerControl.FindForm();
            if (form != null)
                return form.Site;

            return null;
        }

        [ListBindable(false)]
        public class NiTaskDialogButtonCollection : Collection<NiTaskDialogButton>
        {
            public NiTaskDialogButton Add(string text)
            {
                return Add(text, DialogResult.None);
            }

            public NiTaskDialogButton Add(string text, DialogResult dialogResult)
            {
                return Add(text, DialogResult.None, false);
            }

            public NiTaskDialogButton Add(string text, DialogResult dialogResult, bool @default)
            {
                var result = new NiTaskDialogButton
                {
                    Text = text,
                    DialogResult = dialogResult,
                    Default = @default
                };

                Add(result);

                return result;
            }
        }

        private class Notify : NiEventSink, INiTaskDialogNotify
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(Notify));

            private readonly NiTaskDialog _taskDialog;

            public Notify(NiTaskDialog taskDialog, INiConnectionPoint connectionPoint)
                : base(connectionPoint)
            {
                _taskDialog = taskDialog;
            }

            public void OnButtonClick(INiActiveTaskDialog taskDialog, int id, ref bool close)
            {
                try
                {
                    var e = new NiTaskDialogButtonClickEventArgs(taskDialog, _taskDialog.FindButton(id));
                    e.Close = close;

                    _taskDialog.OnButtonClick(e);

                    close = e.Close;
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnCreated(INiActiveTaskDialog taskDialog)
            {
                try
                {
                    _taskDialog.OnCreate(new NiTaskDialogEventArgs(taskDialog));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnDestroyed(INiActiveTaskDialog taskDialog)
            {
                try
                {
                    _taskDialog.OnDestroy(new NiTaskDialogEventArgs(taskDialog));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnDialogConstructed(INiActiveTaskDialog taskDialog)
            {
                try
                {
                    _taskDialog.OnCreateDialog(new NiTaskDialogEventArgs(taskDialog));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnExpandoButtonClicked(INiActiveTaskDialog taskDialog, bool expanded)
            {
                try
                {
                    _taskDialog.OnExpandoButtonClick(new NiTaskDialogExpandoButtonClickEventArgs(
                        taskDialog,
                        expanded
                    ));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnHelp(INiActiveTaskDialog taskDialog)
            {
                try
                {
                    _taskDialog.OnHelp(new NiTaskDialogEventArgs(taskDialog));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnHyperlinkClicked(INiActiveTaskDialog taskDialog, string hyperlink)
            {
                try
                {
                    _taskDialog.OnHyperlinkClick(new NiTaskDialogHyperlinkClickEventArgs(
                        taskDialog,
                        hyperlink
                    ));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnRadioButtonClicked(INiActiveTaskDialog taskDialog, int id)
            {
                try
                {
                    _taskDialog.OnRadioButtonClick(new NiTaskDialogButtonClickEventArgs(
                        taskDialog,
                        _taskDialog.FindButton(id)
                    ));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnTimer(INiActiveTaskDialog taskDialog, int elapsed, ref bool resetTimer)
            {
                try
                {
                    var e = new NiTaskDialogTickEventArgs(taskDialog, elapsed);
                    e.ResetTimer = resetTimer;

                    _taskDialog.OnTick(e);

                    resetTimer = e.ResetTimer;
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }

            public void OnVerificationClicked(INiActiveTaskDialog taskDialog, bool @checked)
            {
                try
                {
                    _taskDialog.OnVerificationFlagCheckedChanged(new NiTaskDialogVerificationFlagCheckedChangedEventArgs(
                        taskDialog,
                        @checked
                    ));
                }
                catch (Exception ex)
                {
                    Log.Warn("Exception from task dialog notify", ex);
                }
            }
        }
    }
}
