using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetIde.Update;
using NetIde.Xml.BuildConfiguration;

namespace NetIde.BuildTasks
{
    partial class NetIdeTask
    {
        private void ExecuteInstallPackages(BuildConfiguration configuration)
        {
            using (new AssemblyResolver())
            {
                foreach (var installPackage in configuration.InstallPackages)
                {
                    try
                    {
                        var installer = new PackageInstaller(
                            installPackage.Context,
                            TranslatePath(installPackage.Package)
                        );

                        installer.Execute();
                    }
                    catch (Exception ex)
                    {
                        Log.LogErrorFromException(ex);
                    }
                }
            }
        }

        private class AssemblyResolver : IDisposable
        {
            private bool _disposed;

            public AssemblyResolver()
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            }

            Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                // This is a hack to ensure that we're able to load the package.
                // The problem is that the package references NetIde.Shell.Interop.1.0.
                // We do too and we need to be able to cast the package class
                // from the package to an INiPackage, which is in that assembly.
                // However, the physical NetIde.Shell.Interop.1.0 that is in
                // the package is different from ours, so .NET will not match
                // up the assemblies. We solve this by returning our
                // NetIde.Shell.Interop.1.0 when the assembly is loaded, which makes
                // .NET think that it's the same assembly.

                return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(
                    p => p.GetName().ToString() == args.Name
                );
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;

                    _disposed = true;
                }
            }
        }
    }
}
