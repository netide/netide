using NetIde.Core.ToolWindows.ProjectExplorer;
using NetIde.Shell;
using NetIde.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.Services.ProjectExplorer
{
    internal class NiProjectExplorer : ServiceBase, INiProjectExplorer
    {
        private readonly CorePackage _package;
        private ProjectExplorerWindow _window;

        public NiProjectExplorer(CorePackage package)
            : base(package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            _package = package;
        }

        public HResult Show()
        {
            try
            {
                if (_window == null)
                    _window = (ProjectExplorerWindow)_package.CreateToolWindow(typeof(ProjectExplorerWindow));

                return _window.Frame.Show();
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Hide()
        {
            try
            {
                if (_window != null)
                    return _window.Frame.Hide();

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSelectedHierarchy(out INiHierarchy hier)
        {
            hier = null;

            try
            {
                if (_window != null)
                    return _window.GetSelectedHierarchy(out hier);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
