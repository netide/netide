using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.CommandManager.Controls;
using NetIde.Services.ProjectManager;
using NetIde.Services.WindowPaneSelection;
using NetIde.Shell;
using NetIde.Shell.Interop;
using WeifenLuo.WinFormsUI.Docking;
using DockContent = NetIde.Services.Shell.DockContent;
using ToolStrip = System.Windows.Forms.ToolStrip;

namespace NetIde
{
    internal partial class MainForm : NetIde.Util.Forms.Form
    {
        private DockContent _lastActiveDockContent;
        private NiWindowPaneSelection _windowPaneSelection;

        public bool AllowQuit { get; set; }
        public bool IsDisposing { get; private set; }

        public INiWindowPane ActiveDocument
        {
            get
            {
                if (_lastActiveDockContent == null)
                    return null;

                return _lastActiveDockContent.WindowPane;
            }
        }

        public MainForm()
        {
            InitializeComponent();

            _dockPanel.Theme = new VS2012LightTheme();
        }

        public override ISite Site
        {
            get
            {
                return base.Site;
            }
            set
            {
                base.Site = value;

                _windowPaneSelection = (NiWindowPaneSelection)GetService(typeof(INiWindowPaneSelection));

                ((IServiceContainer)GetService(typeof(IServiceContainer))).AddService(
                    typeof(INiStatusBar),
                    new NiStatusBar(this, value)
                );
            }
        }

        internal void InsertCommandBar(BarControl commandBar)
        {
            if (commandBar.Bar.Kind == NiCommandBarKind.Menu)
                InsertMenu(commandBar, (ToolStripMenuItem)commandBar.Control);
            else
                InsertToolBar(commandBar, (ToolStrip)commandBar.Control);
        }

        private void InsertMenu(BarControl commandBar, ToolStripMenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException("menuItem");

            var items = _menuStrip.Items;
            int insertIndex = items.Count;

            for (int i = 0; i < items.Count; i++)
            {
                if (((BarControl)items[i].Tag).Bar.Priority > commandBar.Bar.Priority)
                {
                    insertIndex = i;
                    break;
                }
            }

            _menuStrip.Items.Insert(insertIndex, menuItem);
        }

        private void InsertToolBar(BarControl commandBar, ToolStrip toolStrip)
        {
            var toolStripPanel = _toolStripContainer.TopToolStripPanel;

            var toolStrips = toolStripPanel.Controls
                .Cast<ToolStrip>()
                .OrderBy(p => p.Top)
                .ThenBy(p => p.Left)
                .ToList();

            if (toolStrips.Count > 0)
            {
                int rows = toolStripPanel.Rows.Length;

                toolStripPanel.Join(
                    toolStrip,
                    new Point(
                        Math.Min(toolStrips[toolStrips.Count - 1].Right, toolStripPanel.Width),
                        toolStrips[toolStrips.Count - 1].Top
                    )
                );

                if (rows != toolStripPanel.Rows.Length)
                {
                    toolStripPanel.Controls.Remove(toolStrip);
                    toolStripPanel.Join(toolStrip, rows);
                }
            }
            else
            {
                toolStripPanel.Join(toolStrip);
            }
        }

        internal void ShowContent(DockContent dockContent)
        {
            dockContent.Show(
                _dockPanel,
                GetDockState(dockContent.DockStyle, dockContent.Orientation)
            );
        }

        private DockState GetDockState(Shell.Interop.NiDockStyle dockStyle, NiToolWindowOrientation orientation)
        {
            switch (dockStyle)
            {
                case NiDockStyle.Document:
                    return DockState.Document;

                case NiDockStyle.Float:
                case NiDockStyle.AlwaysFloat:
                    return DockState.Float;

                default:
                    switch (orientation)
                    {
                        case NiToolWindowOrientation.Bottom:
                            return DockState.DockBottom;

                        case NiToolWindowOrientation.Left:
                            return DockState.DockLeft;

                        case NiToolWindowOrientation.Right:
                            return DockState.DockRight;

                        default:
                            return DockState.DockTop;
                    }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AllowQuit)
            {
                e.Cancel = true;

                ErrorUtil.ThrowOnFailure(((INiEnv)GetService(typeof(INiEnv))).Quit());
            }
        }

        private void _dockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            if (_lastActiveDockContent != null)
            {
                if (!_lastActiveDockContent.IsDisposed)
                    _lastActiveDockContent.RaiseShow(NiWindowShow.Deactivate);

                _lastActiveDockContent = null;
            }

            _lastActiveDockContent = (DockContent)_dockPanel.ActiveDocument;
            _windowPaneSelection.ActiveDocument =
                _lastActiveDockContent == null
                ? null
                : _lastActiveDockContent.WindowPane;

            if (_lastActiveDockContent != null)
                _lastActiveDockContent.RaiseShow(NiWindowShow.Activate);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            ErrorUtil.ThrowOnFailure(((NiProjectManager)GetService(typeof(INiProjectManager))).OpenProjectFromCommandLine());
        }

        internal HResult GetDocumentWindowIterator(out INiIterator<INiWindowFrame> iterator)
        {
            iterator = null;

            try
            {
                iterator = new NiDocumentWindowIterator(((IEnumerable<IDockContent>)_dockPanel.Documents.ToArray()).GetEnumerator());

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        internal class NiDocumentWindowIterator : NiIterator<IDockContent, INiWindowFrame>
        {
            public NiDocumentWindowIterator(IEnumerator<IDockContent> documents)
                : base(documents)
            {
            }

            protected override INiWindowFrame GetCurrentFromInput(IDockContent current)
            {
                return ((DockContent)current).GetProxy();
            }
        }
    }
}
