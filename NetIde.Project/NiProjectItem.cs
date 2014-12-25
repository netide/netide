using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public class NiProjectItem : NiHierarchy, INiProjectItem
    {
        private string _fileName;

        public HResult GetFileName(out string fileName)
        {
            fileName = _fileName;
            return HResult.OK;
        }

        public HResult SetFileName(string fileName)
        {
            _fileName = fileName;
            return HResult.OK;
        }

        private INiProject FindProject()
        {
            var project = ((INiProject)this.GetPropertyEx(NiHierarchyProperty.ContainingProject));

            if (project == null)
                throw new InvalidOperationException(NeutralResources.CannotResolveProject);

            return project;
        }

        public HResult Open(out INiWindowFrame windowFrame)
        {
            windowFrame = null;

            try
            {
                return FindProject().OpenItem(this, out windowFrame);
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Remove()
        {
            try
            {
                return FindProject().RemoveItem(this);
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Save(string fileName)
        {
            throw new NotImplementedException();
        }

        public HResult SaveAs(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
