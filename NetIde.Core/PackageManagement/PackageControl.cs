using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Core.PackageManagement
{
    internal partial class PackageControl : NetIde.Util.Forms.UserControl
    {
        private bool _isSelected;
        private PackageControlButton? _primaryButton;

        public PackageMetadata Package { get; private set; }

        public event PackageControlButtonEventHandler ButtonClick;

        protected virtual void OnButtonClick(PackageControlButtonEventArgs e)
        {
            var ev = ButtonClick;
            if (ev != null)
                ev(this, e);
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    TabStop = value;

                    foreach (var button in _panel.Controls.OfType<Button>())
                    {
                        button.Visible = _isSelected;
                    }

                    if (value)
                    {
                        Focus();

                        foreach (PackageControl control in Parent.Controls)
                        {
                            if (control != this)
                                control.IsSelected = false;
                        }

                        BackColor = SystemColors.Highlight;
                    }
                    else
                    {
                        BackColor = Color.Empty;
                    }

                    OnIsSelectedChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler IsSelectedChanged;

        protected virtual void OnIsSelectedChanged(EventArgs e)
        {
            var ev = IsSelectedChanged;
            if (ev != null)
                ev(this, e);
        }

        [DefaultValue(false)]
        public new bool TabStop
        {
            get { return base.TabStop; }
            set { base.TabStop = value; }
        }

        public PackageControl(PackageMetadata package, PackageCategory category)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            SetStyle(ControlStyles.Selectable | ControlStyles.ResizeRedraw, true);
            TabStop = false;

            Package = package;

            InitializeComponent();

            _icon.Image =
            _icon.ErrorImage =
            _icon.InitialImage =
                NeutralResources.NuGetPackage;

            HookMouseEvents(this);

            if (!String.IsNullOrEmpty(package.IconUrl))
                PackageImageCache.LoadImage(package.IconUrl, _icon);

            _title.Text = Normalize(package.Title);
            _description.Text = Normalize(package.Description);

            bool isCorePackage = package.State.HasFlag(PackageState.CorePackage);

            switch (category)
            {
                case PackageCategory.Installed:
                    if (package.State.HasFlag(PackageState.Disabled))
                        AddButton(Labels.Enable, PackageControlButton.Enable, 0, !isCorePackage);
                    else
                        AddButton(Labels.Disable, PackageControlButton.Disable, 0, !isCorePackage);

                    AddButton(Labels.Uninstall, PackageControlButton.Uninstall, 1, !isCorePackage && !package.State.HasFlag(PackageState.UninstallPending));
                    break;

                case PackageCategory.Online:
                    bool installedOrPending =
                        package.State.HasFlag(PackageState.Installed) ||
                        package.State.HasFlag(PackageState.InstallPending);

                    AddButton(Labels.Install, PackageControlButton.Install, 0, !installedOrPending);
                    break;

                case PackageCategory.Updates:
                    AddButton(Labels.Update, PackageControlButton.Update, 0, !package.State.HasFlag(PackageState.UpdatePending));
                    break;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            IsSelected = true;
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Focused)
                ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
        }

        private static string Normalize(string text)
        {
            if (String.IsNullOrEmpty(text))
                return null;

            return Regex.Replace(text, "\\s+", " ").Trim();
        }

        private void HookMouseEvents(Control control)
        {
            control.MouseUp += control_MouseUp;
            control.MouseDoubleClick += control_MouseDoubleClick;

            foreach (Control child in control.Controls)
            {
                HookMouseEvents(child);
            }
        }

        void control_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                IsSelected = true;
        }

        void control_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (
                e.Button == MouseButtons.Left &&
                _primaryButton.HasValue
            )
                OnButtonClick(new PackageControlButtonEventArgs(Package, _primaryButton.Value));
        }

        private void AddButton(string label, PackageControlButton button, int index, bool enabled)
        {
            if (index == 0)
                _primaryButton = button;

            var control = new Button
            {
                Text = label,
                FlatStyle = FlatStyle.System,
                Size = new Size(75, 23),
                Tag = button,
                Visible = false,
                TabStop = false,
                Enabled = enabled
            };

            control.Click += control_Click;

            _panel.SetColumn(control, 1);
            _panel.SetRow(control, index);

            _panel.Controls.Add(control);

            _panel.MinimumSize = new Size(0, (control.Height + control.Margin.Vertical) * 2);
        }

        void control_Click(object sender, EventArgs e)
        {
            OnButtonClick(new PackageControlButtonEventArgs(
                Package,
                (PackageControlButton)((Button)sender).Tag
            ));
        }
    }
}
