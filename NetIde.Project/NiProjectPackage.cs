using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public abstract class NiProjectPackage : NiPackage
    {
        public void RegisterProjectFactory(INiProjectFactory projectFactory)
        {
            if (projectFactory == null)
                throw new ArgumentNullException("projectFactory");

            var projectManager = (INiProjectManager)GetService(typeof(INiProjectManager));

            projectFactory.SetSite(this);

            ErrorUtil.ThrowOnFailure(projectManager.RegisterProjectFactory(
                projectFactory.GetType().GUID,
                projectFactory
            ));
        }
    }
}
