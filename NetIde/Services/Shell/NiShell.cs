using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.Env;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;
using NetIde.Win32;

namespace NetIde.Services.Shell
{
    internal class NiShell : ServiceBase, INiShell
    {
        private readonly Dictionary<INiWindowPane, DockContent> _dockContents = new Dictionary<INiWindowPane, DockContent>();
        private readonly NiEnv _env;

        public NiShell(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            _env = (NiEnv)GetService(typeof(INiEnv));
        }

        public HResult CreateToolWindow(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation toolWindowOrientation, out INiWindowFrame toolWindow)
        {
            toolWindow = null;

            try
            {
                if (windowPane == null)
                    throw new ArgumentNullException("windowPane");

                var dockContent = new DockContent(windowPane, dockStyle, toolWindowOrientation)
                {
                    Site = new SiteProxy(this)
                };

                dockContent.Disposed += dockContent_Disposed;

                _dockContents.Add(windowPane, dockContent);

                toolWindow = dockContent.GetProxy();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        void dockContent_Disposed(object sender, EventArgs e)
        {
            _dockContents.Remove(((DockContent)sender).WindowPane);
        }

        public HResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult result;
            return ShowMessageBox(text, caption, buttons, icon, out result);
        }

        public HResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, out DialogResult result)
        {
            result = DialogResult.None;

            try
            {
                result = MessageBox.Show(
                    GetActiveWindow(),
                    text,
                    caption ?? _env.MainWindow.Caption,
                    buttons,
                    icon
                );

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult BrowseForFolder(string title, NiBrowseForFolderOptions options, out string selectedFolder)
        {
            selectedFolder = null;

            try
            {
                var browser = new FolderBrowser
                {
                    Title = title,
                    ShowEditBox = (options & NiBrowseForFolderOptions.HideEditBox) == 0,
                    ShowNewFolderButton = (options & NiBrowseForFolderOptions.HideNewFolderButton) == 0
                };

                if (browser.ShowDialog(GetActiveWindow()) == DialogResult.OK)
                {
                    selectedFolder = browser.SelectedPath;

                    return HResult.OK;
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public IWin32Window GetActiveWindow()
        {
            return new NativeWindowWrapper(NativeMethods.GetActiveWindow());
        }

        public HResult SaveDocDataToFile(NiSaveMode mode, INiPersistFile persistFile, string fileName, out string newFileName, out bool saved)
        {
            throw new NotImplementedException();
        }

        public INiWindowFrame GetFrameOfPane(INiWindowPane pane)
        {
            // TODO: Shouldn't this be a public API point?

            DockContent dockContent;
            if (_dockContents.TryGetValue(pane, out dockContent))
                return dockContent.GetProxy();

            return null;
        }

        private class NativeWindowWrapper : IWin32Window
        {
            public IntPtr Handle { get; private set; }

            public NativeWindowWrapper(IntPtr handle)
            {
                Handle = handle;
            }
        }
    }
}
