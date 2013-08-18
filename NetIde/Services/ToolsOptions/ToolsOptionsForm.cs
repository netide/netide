using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.LocalRegistry;
using NetIde.Services.PackageManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Support;
using NetIde.Util.Forms;

namespace NetIde.Services.ToolsOptions
{
    internal partial class ToolsOptionsForm : DialogForm
    {
        private OptionPage _currentPage;
        private NiLocalRegistry _localRegistry;
        private readonly List<OptionPage> _pages = new List<OptionPage>();
        private static string _lastSelectedCategory;
        private static string _lastSelectedPage;
        private readonly Dictionary<string, TreeNode> _categoryNodes = new Dictionary<string, TreeNode>();

        public ToolsOptionsForm()
        {
            InitializeComponent();

            _treeView.ApplyExplorerTheme();

            Disposed += ToolsOptionsDialog_Disposed;
        }

        void ToolsOptionsDialog_Disposed(object sender, EventArgs e)
        {
            foreach (var page in _pages)
            {
                var disposable = page.Page as IDisposable;

                if (disposable != null)
                    disposable.Dispose();

                page.Page = null;
            }
        }

        private void ToolsOptionsDialog_Load(object sender, EventArgs e)
        {
            _localRegistry = (NiLocalRegistry)GetService(typeof(INiLocalRegistry));

            var packageManager = (NiPackageManager)GetService(typeof(INiPackageManager));

            foreach (var registration in _localRegistry.Registrations.OfType<OptionPageRegistration>())
            {
                var optionPage = new OptionPage(registration, packageManager.Packages[registration.Package]);

                _pages.Add(optionPage);

                var categoryNode = GetCategoryNode(optionPage);

                var treeNode = new TreeNode
                {
                    Text = GetString(optionPage, registration.PageName, registration.PageNameResourceKey),
                    Tag = optionPage
                };

                InsertPageSorted(categoryNode.Nodes, treeNode);

                if (
                    registration.CategoryName == _lastSelectedCategory &&
                    registration.PageName == _lastSelectedPage
                )
                    _treeView.SelectedNode = treeNode;
            }

            if (_treeView.SelectedNode == null && _treeView.Nodes.Count > 0)
                _treeView.SelectedNode = _treeView.Nodes[0].Nodes[0];

            if (_treeView.SelectedNode != null)
                _treeView.SelectedNode.EnsureVisible();
        }

        private void InsertPageSorted(TreeNodeCollection nodes, TreeNode treeNode)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (ComparePage(nodes[i], treeNode) < 0)
                {
                    nodes.Insert(i, treeNode);
                    return;
                }
            }

            nodes.Add(treeNode);
        }

        private int ComparePage(TreeNode a, TreeNode b)
        {
            var aPage = (OptionPage)a.Tag;
            var bPage = (OptionPage)b.Tag;

            int result = CompareSpecial(aPage.Registration.PageName, bPage.Registration.PageName, "General");

            if (result != 0)
                return result;

            return String.Compare(a.Text, b.Text, StringComparison.OrdinalIgnoreCase);
        }

        private int CompareSpecial(string a, string b, params string[] specialNames)
        {
            int aResult = Array.IndexOf(specialNames, a);
            int bResult = Array.IndexOf(specialNames, b);

            if (aResult == -1)
                aResult = int.MaxValue;
            if (bResult == -1)
                bResult = int.MaxValue;

            return -aResult.CompareTo(bResult);
        }

        private string GetString(OptionPage page, string fallback, string resourceId)
        {
            if (String.IsNullOrEmpty(resourceId))
                return fallback;

            string result;
            ErrorUtil.ThrowOnFailure(page.Package.Package.GetStringResource(resourceId, out result));

            if (String.IsNullOrEmpty(result))
                return fallback;

            return result;
        }

        private TreeNode GetCategoryNode(OptionPage page)
        {
            TreeNode treeNode;

            if (_categoryNodes.TryGetValue(page.Registration.CategoryName, out treeNode))
                return treeNode;

            treeNode = new TreeNode
            {
                Text = GetString(page, page.Registration.CategoryName, page.Registration.CategoryNameResourceKey),
                Tag = new OptionCategory(page.Registration.CategoryName)
            };

            InsertCategorySorted(_treeView.Nodes, treeNode);

            _categoryNodes[page.Registration.CategoryName] = treeNode;

            return treeNode;
        }

        private void InsertCategorySorted(TreeNodeCollection nodes, TreeNode treeNode)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (CompareCategory(nodes[i], treeNode) < 0)
                {
                    nodes.Insert(i, treeNode);
                    return;
                }
            }

            nodes.Add(treeNode);
        }

        private int CompareCategory(TreeNode a, TreeNode b)
        {
            var aPage = (OptionCategory)a.Tag;
            var bPage = (OptionCategory)b.Tag;

            int result = CompareSpecial(aPage.CategoryName, bPage.CategoryName, "Environment");

            if (result != 0)
                return result;

            return String.Compare(a.Text, b.Text, StringComparison.OrdinalIgnoreCase);
        }

        private void _treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            var newPage = (OptionPage)(e.Node.Nodes.Count > 0 ? e.Node.Nodes[0] : e.Node).Tag;

            if (_currentPage == newPage)
                return;

            if (!CanDeactivate())
            {
                e.Cancel = true;
                return;
            }

            _currentPage = newPage;

            if (_currentPage.Page == null)
            {
                object instance;
                ErrorUtil.ThrowOnFailure(_localRegistry.CreateInstance(_currentPage.Registration.Id, out instance));

                _currentPage.Page = (INiOptionPage)instance;

                ErrorUtil.ThrowOnFailure(_currentPage.Page.SetSite(_currentPage.Package.Package));
                ErrorUtil.ThrowOnFailure(_currentPage.Page.Initialize());
            }

            _pageHost.Page = _currentPage.Page;

            ErrorUtil.ThrowOnFailure(_currentPage.Page.Activate());

            _lastSelectedCategory = _currentPage.Registration.CategoryName;
            _lastSelectedPage = _currentPage.Registration.PageName;
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            if (!CanDeactivate())
                return;

            foreach (var page in _pages)
            {
                if (page.Page != null)
                    ErrorUtil.ThrowOnFailure(page.Page.Apply());
            }

            DialogResult = DialogResult.OK;
        }

        private bool CanDeactivate()
        {
            if (_currentPage != null)
            {
                bool canDeactivate;
                ErrorUtil.ThrowOnFailure(_currentPage.Page.Deactivate(out canDeactivate));

                return canDeactivate;
            }

            return true;
        }

        private void ToolsOptionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel)
            {
                foreach (var page in _pages)
                {
                    if (page.Page != null)
                        ErrorUtil.ThrowOnFailure(page.Page.Cancel());
                }
            }
        }

        private void ToolsOptionsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = !CanDeactivate();
        }

        private class OptionPage
        {
            public OptionPageRegistration Registration { get; private set; }
            public PackageRegistration Package { get; private set; }
            public INiOptionPage Page { get; set; }

            public OptionPage(OptionPageRegistration registration, PackageRegistration package)
            {
                Registration = registration;
                Package = package;
            }
        }

        private class OptionCategory
        {
            public string CategoryName { get; private set; }

            public OptionCategory(string categoryName)
            {
                CategoryName = categoryName;
            }
        }
    }
}
