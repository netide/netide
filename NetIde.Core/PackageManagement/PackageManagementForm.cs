using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Core.Support;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Update;
using NetIde.Xml.PackageMetadata;
using log4net;

namespace NetIde.Core.PackageManagement
{
    internal partial class PackageManagementForm : BaseForm
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PackageManagementForm));
        private static readonly Padding SelectorItemPadding = new Padding(6, 6, 6, 6);
        private const TextFormatFlags SelectorTextFlags = TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

        private static bool _restartPending;

        private INiEnv _env;
        private int _cookie;
        private PackageCategory _lastCategory;

        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;

                _env = (INiEnv)GetService(typeof(INiEnv));
            }
        }

        public PackageManagementForm()
        {
            InitializeComponent();

            _selector.Items.AddRange(new object[]
            {
                SelectionItem.Create(PackageCategory.Installed, Labels.Installed),
                SelectionItem.Create(PackageCategory.Online, Labels.Online),
                SelectionItem.Create(PackageCategory.Updates, Labels.Updates)
            });

            _packageDetails.Visible = false;
        }

        private void _selector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = ((SelectionItem<PackageCategory>)_selector.SelectedItem).Value;

            if (category == _lastCategory)
                return;

            _lastCategory = category;

            LoadCategory(category);
        }

        private void LoadCategory(PackageCategory category)
        {
            switch (category)
            {
                case PackageCategory.Installed:
                    LoadInstalledPackages();
                    break;

                case PackageCategory.Online:
                    LoadOnlinePackages();
                    break;

                case PackageCategory.Updates:
                    LoadPackagesForUpdate();
                    break;
            }
        }

        private void LoadInstalledPackages()
        {
            DisplayPackages(++_cookie, PackageRegistry.GetInstalledPackages(_env.Context));

            SetLoading(false);
        }

        private void LoadOnlinePackages()
        {
            int cookie = ++_cookie;

            SetLoading(true);

            ThreadPool.QueueUserWorkItem(p => PerformLoadOnlinePackages(cookie));
        }

        private void PerformLoadOnlinePackages(int cookie)
        {
            try
            {
                var packages = NuGetQuerier.Query(
                    _env.Context,
                    _env.NuGetSite,
                    _packageList.SelectedStability,
                    _packageList.SelectedQueryOrder,
                    _packageList.SelectedPage
                );

                BeginInvoke(new Action(() => DisplayPackages(cookie, packages)));
            }
            catch (Exception ex)
            {
                Log.Warn("Could not load online packages", ex);
            }
        }

        private void LoadPackagesForUpdate()
        {
            int cookie = ++_cookie;

            SetLoading(true);

            ThreadPool.QueueUserWorkItem(p => PerformLoadPackagesForUpdate(cookie));
        }

        private void PerformLoadPackagesForUpdate(int cookie)
        {
            var installedPackages = PackageRegistry.GetInstalledPackages(_env.Context);

            var packages = NuGetQuerier.Query(
                _env.Context,
                _env.NuGetSite,
                PackageStability.StableOnly,
                installedPackages.Packages
            );

            BeginInvoke(new Action(() => DisplayPackages(cookie, packages)));
        }

        private void DisplayPackages(int cookie, PackageQueryResult packages)
        {
            if (_cookie != cookie)
                return;

            _packageList.RestartPending = _restartPending;

            switch (_lastCategory)
            {
                case PackageCategory.Installed:
                    _packageList.NoResultsText = Labels.NoPackagesInstalled;
                    break;

                case PackageCategory.Online:
                    _packageList.NoResultsText = Labels.NoPackagesAvailable;
                    break;

                case PackageCategory.Updates:
                    _packageList.NoResultsText = Labels.NoUpdatesAvailable;
                    break;
            }

            _packageList.LoadPackages(packages, _lastCategory);

            SetLoading(false);
        }

        private void SetLoading(bool loading)
        {
            if (loading)
                _packageDetails.Visible = false;

            _packageList.ShowToolbar = _lastCategory == PackageCategory.Online;

            _packageList.Loading = loading;
        }

        void _packageList_SelectedPackageChanged(object sender, EventArgs e)
        {
            var package = _packageList.SelectedPackage;

            _packageDetails.Visible = package != null;
            _packageDetails.Package = package;
        }

        private void PackageManagementForm_Load(object sender, EventArgs e)
        {
            _selector.SelectedIndex = 1;
        }

        private void _cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _packageList_QueryParametersChanged(object sender, EventArgs e)
        {
            if (_lastCategory == PackageCategory.Online)
                LoadOnlinePackages();
        }

        private void _selector_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            var item = (SelectionItem<PackageCategory>)_selector.Items[e.Index];

            var size = TextRenderer.MeasureText(
                item.Text,
                Font,
                new Size(int.MaxValue, int.MaxValue),
                SelectorTextFlags
            );

            e.ItemHeight = size.Height + SelectorItemPadding.Vertical;
        }

        private void _selector_DrawItem(object sender, DrawItemEventArgs e)
        {
            var item = (SelectionItem<PackageCategory>)_selector.Items[e.Index];

            e.DrawBackground();

            TextRenderer.DrawText(
                e.Graphics,
                item.Text,
                Font,
                new Rectangle(
                    e.Bounds.Left + SelectorItemPadding.Left,
                    e.Bounds.Top + SelectorItemPadding.Top,
                    e.Bounds.Width - SelectorItemPadding.Horizontal,
                    e.Bounds.Height - SelectorItemPadding.Vertical
                ),
                e.ForeColor,
                e.BackColor,
                SelectorTextFlags
            );

            e.DrawFocusRectangle();
        }

        private void _packageList_PackageButtonClick(object sender, PackageControlButtonEventArgs e)
        {
            _restartPending = true;

            switch (e.Button)
            {
                case PackageControlButton.Enable:
                case PackageControlButton.Disable:
                    PackageRegistry.EnablePackage(_env.Context, e.Package.Id, e.Button == PackageControlButton.Enable);
                    break;

                case PackageControlButton.Uninstall:
                    PackageRegistry.QueueUninstall(_env.Context, e.Package.Id);
                    break;

                case PackageControlButton.Update:
                case PackageControlButton.Install:
                    QueueUpdate(e.Package);
                    break;
            }

            LoadCategory(_lastCategory);
        }

        private void QueueUpdate(PackageMetadata package)
        {
            PackageRegistry.QueueUpdate(_env.Context, package);

            foreach (var dependency in package.Dependencies)
            {
                var dependentPackage = NuGetQuerier.ResolvePackageVersion(_env.Context, _env.NuGetSite, dependency.Id, dependency.Version, PackageStability.StableOnly);

                if (dependentPackage != null)
                    QueueUpdate(dependentPackage);
            }
        }

        private void _packageList_RestartClick(object sender, EventArgs e)
        {
            if (NiShellUtil.Confirm(Site))
                ErrorUtil.ThrowOnFailure(_env.RestartApplication());
        }
    }
}
