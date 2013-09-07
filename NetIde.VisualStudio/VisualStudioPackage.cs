using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace NetIde.VisualStudio
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid(GuidList.VisualStudioPackage)]
    public sealed class VisualStudioPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();
        }
    }
}
