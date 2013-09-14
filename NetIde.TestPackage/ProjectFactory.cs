using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.TestPackage.Api;

namespace NetIde.TestPackage
{
    [Guid(TPResources.ProjectGuid)]
    public class ProjectFactory : NiProjectFactory
    {
        public override HResult CreateProject(string fileName, NiProjectCreateMode createMode, out INiProject project)
        {
            project = null;

            try
            {
                project = LoadProject(fileName, createMode);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private INiProject LoadProject(string fileName, NiProjectCreateMode createMode)
        {
            var project = new Project();

            project.SetSite(this);
            project.Load(fileName, createMode);

            return project;
        }
    }
}
