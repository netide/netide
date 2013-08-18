using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Support;
using NetIde.Update;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Core.PackageManagement
{
    internal partial class PackageListControl : NetIde.Util.Forms.UserControl
    {
        private bool _unSelectPending;
        private PackageMetadata _selectedPackage;
        private PackageStability _selectedStability;
        private PackageQueryOrder _selectedQueryOrder;
        private int _selectedPage;
        private Font _boldFont;
        private bool _loading;
        private string _noResultsText;

        public bool ShowToolbar
        {
            get { return _toolbar.Visible; }
            set { _toolbar.Visible = value; }
        }

        public bool Loading
        {
            get { return _loading; }
            set
            {
                if (_loading != value)
                {
                    _loading = value;

                    if (value)
                        _loadingControl.BringToFront();
                    else
                        _loadingControl.SendToBack();
                }
            }
        }

        public PackageMetadata SelectedPackage
        {
            get { return _selectedPackage; }
            private set 
            {
                if (_selectedPackage != value)
                {
                    _selectedPackage = value;
                    OnSelectedPackageChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler SelectedPackageChanged;

        protected virtual void OnSelectedPackageChanged(EventArgs e)
        {
            var ev = SelectedPackageChanged;
            if (ev != null)
                ev(this, e);
        }

        public PackageStability SelectedStability
        {
            get { return _selectedStability; }
            private set
            {
                if (_selectedStability != value)
                {
                    _selectedStability = value;
                    OnQueryParametersChanged(EventArgs.Empty);
                }
            }
        }

        public PackageQueryOrder SelectedQueryOrder
        {
            get { return _selectedQueryOrder; }
            private set
            {
                if (_selectedQueryOrder != value)
                {
                    _selectedQueryOrder = value;
                    OnQueryParametersChanged(EventArgs.Empty);
                }
            }
        }

        public int SelectedPage
        {
            get { return _selectedPage; }
            set
            {
                if (_selectedPage != value)
                {
                    _selectedPage = value;
                    OnQueryParametersChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler QueryParametersChanged;

        protected virtual void OnQueryParametersChanged(EventArgs e)
        {
            var ev = QueryParametersChanged;
            if (ev != null)
                ev(this, e);
        }

        public string NoResultsText
        {
            get { return _noResultsText; }
            set
            {
                if (_noResultsText != value)
                {
                    _noResultsText = value;
                    Invalidate();
                }
            }
        }

        public event PackageControlButtonEventHandler PackageButtonClick;

        protected virtual void OnPackageButtonClick(PackageControlButtonEventArgs e)
        {
            var ev = PackageButtonClick;
            if (ev != null)
                ev(this, e);
        }

        public bool RestartPending
        {
            get { return _restartPanel.Visible; }
            set { _restartPanel.Visible = value; }
        }

        public event EventHandler RestartClick;

        protected virtual void OnRestartClick(EventArgs e)
        {
            var ev = RestartClick;
            if (ev != null)
                ev(this, e);
        }

        public PackageListControl()
        {
            InitializeComponent();

            RestartPending = false;

            foreach (var button in _pager.Controls.OfType<PackagePageButton>())
            {
                if (button != _page1Button)
                    button.Visible = false;
            }

            foreach (Control control in _container.Controls)
            {
                control.Dock = DockStyle.Fill;
            }

            Loading = true;

            _packageStability.Items.AddRange(new object[]
            {
                SelectionItem.Create(PackageStability.StableOnly, Labels.StableOnly),
                SelectionItem.Create(PackageStability.IncludePrerelease, Labels.IncludePrerelease)
            });

            _packageStability.SelectedIndex = 0;

            _sortBy.Items.AddRange(new object[]
            {
                SelectionItem.Create(PackageQueryOrder.MostDownloads, Labels.MostDownloads),
                SelectionItem.Create(PackageQueryOrder.PublishedDate, Labels.PublishedDate),
                SelectionItem.Create(PackageQueryOrder.NameAscending, Labels.NameAscending),
                SelectionItem.Create(PackageQueryOrder.NameDescending, Labels.NameDescending),
            });

            _sortBy.SelectedIndex = 0;

            ReloadBoldFont();

            Disposed += PackageListControl_Disposed;
        }

        void PackageListControl_Disposed(object sender, EventArgs e)
        {
            if (_boldFont != null)
            {
                _boldFont.Dispose();
                _boldFont = null;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            ReloadBoldFont();
        }

        private void ReloadBoldFont()
        {
            if (_boldFont != null)
                _boldFont.Dispose();

            _boldFont = new Font(Font, FontStyle.Bold);
        }

        public void LoadPackages(PackageQueryResult packages, PackageCategory category)
        {
            _packages.SuspendLayout();

            _packages.Controls.Clear();

            foreach (var package in packages.Packages)
            {
                var control = new PackageControl(package, category);

                _packages.Controls.Add(control);

                control.IsSelectedChanged += control_IsSelectedChanged;
                control.ButtonClick += control_ButtonClick;
            }

            if (_packages.Controls.Count > 0)
                ((PackageControl)_packages.Controls[0]).IsSelected = true;

            _packages.ResumeLayout();

            _selectedPage = packages.Page;

            _pager.SuspendLayout();

            _leftButton.Visible = packages.Page > 0;
            _rightButton.Visible = packages.Page < packages.PageCount - 1;

            int start = Math.Max(packages.Page - 2, 0);
            int end = Math.Min(packages.PageCount - 1, start + 4);

            var buttons = new[]
            {
                _page1Button,
                _page2Button,
                _page3Button,
                _page4Button,
                _page5Button
            };

            int visibleCount = (end - start) + 1;

            for (int i = 0; i < 5; i++)
            {
                buttons[i].Visible = i < visibleCount;
            }

            for (int i = start, buttonIndex = 0; i <= end; i++, buttonIndex++)
            {
                buttons[buttonIndex].Font = packages.Page == i ? _boldFont : Font;
                buttons[buttonIndex].Text = (i + 1).ToString();
                buttons[buttonIndex].Tag = i;
            }

            _pager.ResumeLayout();
        }

        void control_ButtonClick(object sender, PackageControlButtonEventArgs e)
        {
            OnPackageButtonClick(e);
        }

        void control_IsSelectedChanged(object sender, EventArgs e)
        {
            var control = (PackageControl)sender;

            if (control.IsSelected)
            {
                _unSelectPending = false;

                SelectedPackage = control.Package;
            }
            else
            {
                if (!_unSelectPending)
                {
                    _unSelectPending = true;
                    BeginInvoke(new Action(PerformUnSelect));
                }
            }
        }

        private void PerformUnSelect()
        {
            if (!_unSelectPending)
                return;

            SelectedPackage = null;
        }

        private void _packageStability_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedStability = ((SelectionItem<PackageStability>)_packageStability.SelectedItem).Value;
        }

        private void _sortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedQueryOrder = ((SelectionItem<PackageQueryOrder>)_sortBy.SelectedItem).Value;
        }

        private void _pageButton_Click(object sender, EventArgs e)
        {
            SelectedPage = (int)((Control)sender).Tag;
        }

        private void _leftButton_Click(object sender, EventArgs e)
        {
            SelectedPage--;
        }

        private void _rightButton_Click(object sender, EventArgs e)
        {
            SelectedPage++;
        }

        private void _packages_Paint(object sender, PaintEventArgs e)
        {
            if (_packages.Controls.Count == 0 && !String.IsNullOrEmpty(NoResultsText))
            {
                TextRenderer.DrawText(
                    e.Graphics,
                    NoResultsText,
                    Font,
                    new Rectangle(0, 0, _packages.ClientSize.Width, 60),
                    ForeColor,
                    BackColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis
                );
            }
        }

        private void _restartButton_Click(object sender, EventArgs e)
        {
            OnRestartClick(EventArgs.Empty);
        }
    }
}
