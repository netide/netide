using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using NetIde.Shell.Interop;
using NetIde.Xml;
using NetIde.Xml.PackageManifest;
using NetIde.Xml.PackageMetadata;
using File = System.IO.File;

namespace NetIde.Update
{
    public class PackageInstaller : PackageManager
    {
        private string _packagePath;

        public PackageInstaller(string context, string packagePath)
            : base(context)
        {
            if (packagePath == null)
                throw new ArgumentNullException("packagePath");

            _packagePath = packagePath;
        }

        public override void Execute()
        {
            if (!File.Exists(_packagePath))
                throw new PackageInstallationException(Labels.InvalidPackagePath, 5);

            // Get the context root.

            var fileSystemRoot = GetFileSystemRoot();

            PackageMetadata metadata;

            using (var zipFile = new ZipFile(_packagePath))
            {
                metadata = ExtractMetadata(zipFile);

                if (metadata == null)
                    throw new PackageInstallationException(Labels.InvalidPackage, 6);

                if (!IsValidPackageId(Context, metadata.Id))
                    throw new PackageInstallationException(Labels.InvalidPackageId, 9);

                // Temporarily store a few parameters we want to add back later.

                using (var contextKey = OpenContextRegistry(false))
                using (var packageKey = contextKey.OpenSubKey("InstalledProducts\\" + metadata.Id))
                {
                    if (packageKey != null)
                    {
                        metadata.NuGetSite = (string)packageKey.GetValue("NuGetSite");
                        metadata.GalleryDetailsUrl = (string)packageKey.GetValue("GalleryDetailsUrl");
                    }
                }
            }

            string target = RemovePackage(metadata.Id, fileSystemRoot);

            ExtractPackage(_packagePath, target);

            var manifest = GetPackageManifest(target);

            if (manifest == null)
                throw new PackageInstallationException(Labels.InvalidPackage, 8);

            _packagePath = target;

            try
            {
                INiPackage package;

                using (CreateAppDomain(_packagePath, manifest.EntryPoint, out package))
                {
                    PerformRegistration(metadata.Id, fileSystemRoot, package);
                }

                // Restore some variables that were removed as part of the update.

                using (var contextKey = OpenContextRegistry(true))
                using (var packageKey = contextKey.CreateSubKey("InstalledProducts\\" + metadata.Id))
                {
                    if (metadata.NuGetSite != null)
                        packageKey.SetValue("NuGetSite", metadata.NuGetSite);
                    if (metadata.GalleryDetailsUrl != null)
                        packageKey.SetValue("GalleryDetailsUrl", metadata.GalleryDetailsUrl);
                }
            }
            catch
            {
                try
                {
                    RemovePackage(metadata.Id, fileSystemRoot);
                }
                catch (Exception ex)
                {
                    throw new PackageInstallationException(String.Format(Labels.RollbackFailed, ex.Message), 4, ex);
                }

                throw;
            }
        }

        private PackageManifest GetPackageManifest(string target)
        {
            string fileName = Path.Combine(target, PackageManifest.FileName);

            if (!File.Exists(fileName))
                return null;

            return Serialization.Deserialize<PackageManifest>(fileName);
        }

        private PackageMetadata ExtractMetadata(ZipFile zipFile)
        {
            foreach (ZipEntry entry in zipFile)
            {
                if (
                    !entry.Name.Contains('/') &&
                    String.Equals(Path.GetExtension(entry.Name), ".nuspec", StringComparison.OrdinalIgnoreCase)
                ) {
                    using (var stream = zipFile.GetInputStream(entry))
                    {
                        var nuSpec = Serialization.Deserialize<NuSpec>(stream);

                        return nuSpec.Metadata;
                    }
                }
            }

            return null;
        }

        private void PerformRegistration(string packageId, string fileSystemRoot, INiPackage package)
        {
            var registrationContext = new NiRegistrationContext(Context, packageId, fileSystemRoot);

            // Unregister when this package is already installed.

            bool isInstalled;

            using (var rootKey = OpenContextRegistry(false))
            using (var packageKey = rootKey.OpenSubKey("InstalledProducts\\" + packageId))
            {
                isInstalled = packageKey != null;
            }

            if (isInstalled)
                Marshal.ThrowExceptionForHR((int)package.Unregister(registrationContext));

            // Perform the registration process.

            Marshal.ThrowExceptionForHR((int)package.Register(registrationContext));
        }

        private static string RemovePackage(string packageId, string fileSystemRoot)
        {
            string target = Path.Combine(fileSystemRoot, "Packages", packageId);

            if (Directory.Exists(target))
                Directory.Delete(target, true);

            return target;
        }
    }
}
