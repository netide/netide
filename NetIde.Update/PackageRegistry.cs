using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Xml;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Update
{
    public static class PackageRegistry
    {
        private const string RegistryRootKey = "Software\\Net IDE";

        internal static RegistryKey OpenRegistryRoot(bool writable, string context)
        {
            string contextKey = RegistryRootKey + "\\" + context;

            if (writable)
                return Registry.CurrentUser.CreateSubKey(contextKey);
            else
                return Registry.CurrentUser.OpenSubKey(contextKey);
        }

        public static PackageQueryResult GetInstalledPackages(string context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var packages = new List<PackageMetadata>();

            using (var contextKey = OpenRegistryRoot(false, context))
            {
                string packagesPath = Path.Combine(
                    (string)contextKey.GetValue("InstallationPath"),
                    "Packages"
                );

                using (var key = contextKey.OpenSubKey("InstalledProducts"))
                {
                    foreach (var packageId in key.GetSubKeyNames())
                    {
                        using (var packageKey = key.OpenSubKey(packageId))
                        {
                            string packagePath = Path.Combine(
                                packagesPath,
                                packageId
                            );

                            var state = GetPackageState(contextKey, context, packageId);

                            if (state.HasFlag(PackageState.Installed))
                                packages.Add(LoadPackage(context, packagePath, packageKey, state));
                        }
                    }
                }
            }

            return new PackageQueryResult(packages.Count, 0, 1, packages);
        }

        private static PackageMetadata LoadPackage(string context, string packagePath, RegistryKey packageKey, PackageState state)
        {
            string[] files = Directory.GetFiles(packagePath, "*.nuspec").ToArray();

            if (files.Length != 1)
                throw new PackageUpdateException(Labels.CouldNotFindNuSpecManifest);

            var nuSpec = Serialization.Deserialize<NuSpec>(files[0]);

            var metadata = nuSpec.Metadata;

            metadata.GalleryDetailsUrl = (string)packageKey.GetValue("GalleryDetailsUrl");
            metadata.NuGetSite = (string)packageKey.GetValue("NuGetSite");
            metadata.Version = (string)packageKey.GetValue("Version");
            metadata.PendingVersion = (string)packageKey.GetValue("PendingVersion");
            metadata.State = state;

            return metadata;
        }

        internal static PackageState GetPackageState(RegistryKey contextKey, string context, string packageId)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (packageId == null)
                throw new ArgumentNullException("packageId");

            using (var key = contextKey.OpenSubKey("InstalledProducts\\" + packageId))
            {
                return GetPackageState(context, packageId, key);
            }
        }

        private static PackageState GetPackageState(string context, string packageId, RegistryKey packageKey)
        {
            PackageState state = 0;

            if (
                String.Equals(packageId, "NetIde.Runtime", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(packageId, "NetIde.Package.Core", StringComparison.OrdinalIgnoreCase) ||
                String.Equals(packageId, context + ".Package.Core", StringComparison.OrdinalIgnoreCase)
            )
                state |= PackageState.CorePackage;

            if (packageKey != null)
            {
                if ((packageKey.GetValue("UninstallPending") as int?).GetValueOrDefault() != 0)
                {
                    state |= PackageState.UninstallPending;
                }
                else
                {
                    if (packageKey.GetValue("Version") != null)
                    {
                        state |= PackageState.Installed;

                        if ((packageKey.GetValue("Disabled") as int?).GetValueOrDefault() != 0)
                            state |= PackageState.Disabled;
                        if (packageKey.GetValue("PendingVersion") != null)
                            state |= PackageState.UpdatePending;
                    }
                    else
                    {
                        state |= PackageState.UpdatePending | PackageState.InstallPending;
                    }
                }
            }

            return state;
        }

        public static void EnablePackage(string context, string packageId, bool enabled)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (packageId == null)
                throw new ArgumentNullException("packageId");

            using (var contextKey = OpenRegistryRoot(true, context))
            using (var key = contextKey.CreateSubKey("InstalledProducts\\" + packageId))
            {
                if (enabled)
                    key.DeleteValue("Disabled");
                else
                    key.SetValue("Disabled", 1);
            }
        }

        public static void QueueUninstall(string context, string packageId)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (packageId == null)
                throw new ArgumentNullException("packageId");

            using (var contextKey = OpenRegistryRoot(true, context))
            using (var packageKey = contextKey.CreateSubKey("InstalledProducts\\" + packageId))
            {
                packageKey.SetValue("UninstallPending", 1);
            }
        }

        public static void QueueUpdate(string context, PackageMetadata metadata)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            using (var contextKey = OpenRegistryRoot(true, context))
            using (var packageKey = contextKey.CreateSubKey("InstalledProducts\\" + metadata.Id))
            {
                packageKey.SetValue("NuGetSite", metadata.NuGetSite);
                packageKey.SetValue("GalleryDetailsUrl", metadata.GalleryDetailsUrl);
                packageKey.SetValue("PendingVersion", metadata.Version);
            }
        }
    }
}
