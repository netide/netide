using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Update;
using NetIde.Xml.BuildConfiguration;

namespace NetIde.BuildTasks
{
    partial class NetIdeTask
    {
        private void ExecuteCreateContext(BuildConfiguration configuration)
        {
            if (configuration.CreateContext == null)
                return;

            string target =
                configuration.CreateContext.Target ??
                TargetDir;

            using (var key = PackageRegistry.OpenRegistryRoot(true, new ContextName(configuration.CreateContext.Context, true)))
            {
                key.SetValue("InstallationPath", target);
            }
        }
    }
}
