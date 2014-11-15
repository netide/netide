using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.ToolWindows.ProjectExplorer
{
    [Guid("70a1a826-334e-4679-bd69-5bf9b32a26cb")]
    internal class ProjectExplorerWindow : NiWindowPane
    {
        private ProjectExplorerControl _control;

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                Frame.Icon = Resources.Folders;
                Frame.Caption = Labels.ProjectExplorer;

                _control = new ProjectExplorerControl
                {
                    Site = new SiteProxy(this),
                    Dock = DockStyle.Fill
                };

                Controls.Add(_control);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult GetSelectedHierarchy(out INiHierarchy hier)
        {
            return _control.GetSelectedHierarchy(out hier);
        }
    }
}
