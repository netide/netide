using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Xml;
using NetIde.Xml.PackageManifest;

namespace NetIde.Update
{
    public class PackageUninstaller : PackageManager
    {
        private readonly string _packageId;

        public PackageUninstaller(ContextName context, string packageId)
            : base(context)
        {
            if (packageId == null)
                throw new ArgumentNullException("packageId");

            _packageId = packageId;
        }

        public override void Execute()
        {
            // Get the context root.

            string fileSystemRoot;
            Guid packageGuid;

            using (var contextKey = OpenContextRegistry(false))
            {
                if (contextKey != null)
                    fileSystemRoot = contextKey.GetValue("InstallationPath") as string;
                else
                    throw new PackageInstallationException(Labels.ContextDoesNotExist, 4);

                using (var key = contextKey.OpenSubKey("InstalledProducts\\" + _packageId))
                {
                    packageGuid = Guid.Parse((string)key.GetValue("Package"));
                }
            }

            string packagePath = Path.Combine(fileSystemRoot, "Packages", _packageId);

            var manifest = Serialization.Deserialize<PackageManifest>(
                Path.Combine(packagePath, PackageManifest.FileName)
            );

            try
            {
                INiPackage package;

                using (CreateAppDomain(packagePath, manifest.EntryPoint, out package))
                {
                    bool isInstalled = PerformUnregistration(fileSystemRoot, package);

                    if (!isInstalled)
                        return;
                }

                RemovePackage(packagePath);
            }
            finally
            {
                // To protect against endless uninstall loops, we unconditionally
                // remove all registration of the package, even when the
                // removal failed.

                using (var contextKey = OpenContextRegistry(true))
                {
                    contextKey.DeleteSubKeyTree("InstalledProducts\\" + _packageId, false);
                    contextKey.DeleteSubKeyTree("Packages\\" + packageGuid.ToString("B").ToUpperInvariant(), false);
                }
            }
        }

        private bool PerformUnregistration(string fileSystemRoot, INiPackage package)
        {
            // Unregister when this package is already installed.

            using (var contextKey = OpenContextRegistry(false))
            using (var key = contextKey.OpenSubKey("InstalledProducts\\" + _packageId))
            {
                if (key == null)
                    return false;
            }

            var registrationContext = new NiRegistrationContext(Context, _packageId, fileSystemRoot);

            Marshal.ThrowExceptionForHR((int)package.Unregister(registrationContext));

            return true;
        }

        private static void RemovePackage(string packagePath)
        {
            if (Directory.Exists(packagePath))
                Directory.Delete(packagePath, true);
        }
    }
}
