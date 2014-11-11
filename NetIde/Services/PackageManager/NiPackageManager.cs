using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NetIde.Services.CommandManager;
using NetIde.Util;
using Microsoft.Win32;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Xml.PackageManifest;
using NetIde.Xml.PackageMetadata;
using log4net;
using File = System.IO.File;
using Serialization = NetIde.Xml.Serialization;

namespace NetIde.Services.PackageManager
{
    internal class NiPackageManager : ServiceBase, INiPackageManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiPackageManager));

        private readonly PackageCollection _packages = new PackageCollection();
        private readonly Dictionary<INiPackage, PackageRegistration> _byPackage = new Dictionary<INiPackage, PackageRegistration>();

        private static readonly Guid CorePackage = new Guid("129646DC-01E6-4443-ABAC-453F731EA7F5");

        public IKeyedCollection<Guid, PackageRegistration> Packages { get; private set; }

        public NiPackageManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Packages = ReadOnlyKeyedCollection.Create(_packages);
        }

        public void Initialize()
        {
            var env = (INiEnv)GetService(typeof(INiEnv));

            ProcessRuntimeUpdate(env);
            RemovePendingUpdate(env);
            ProcessPendingUpdates(env);
            LoadPackages(env);
        }

        private void ProcessRuntimeUpdate(INiEnv env)
        {
            // A runtime update is requested by adding /RuntimeUpdate to
            // the arguments. The command line service knows this and always
            // checks for this argument.

            var commandLine = (INiCommandLine)GetService(typeof(INiCommandLine));

            bool present;
            string value;
            ErrorUtil.ThrowOnFailure(commandLine.GetOption("/RuntimeUpdate", out present, out value));

            if (!present)
                return;

            var pendingUpdateLocation = GetPendingUpdateLocation(env);

            // And we want to copy into the bin folder.

            string target = Path.Combine(env.FileSystemRoot, "bin");

            // Move the current target directory out of the way.

            string tempTarget;
            if (!TryMoveTarget(target, out tempTarget))
            {
                // If we aren't able to move the target out of the way
                // (i.e. can't take ownership of the directory), we can't
                // continue. We got here in the first place because of a
                // pending update to NetIde.Package.Core, so we cancel
                // this and just restart.

                PackageUpdater.CancelCorePackageUpdate(env);
            }
            else
            {
                try
                {
                    CopyDirectory(pendingUpdateLocation, target);

                    // The restart will take care of removing the pending update
                    // location.
                }
                finally
                {
                    // And finally, remove the temporary target directory because
                    // this is of no use anymore.

                    if (tempTarget != null)
                        SEH.SinkExceptions(() => Directory.Delete(tempTarget, true));
                }
            }

            // We still need to update the NetIde.Package.Core package itself.

            // Restart the newly installed runtime.

            string fileName = Path.Combine(
                target,
                Path.GetFileName(GetType().Assembly.Location)
            );

            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                WorkingDirectory = env.FileSystemRoot,
                UseShellExecute = false
            });

            Environment.Exit(0);
        }

        private void RemovePendingUpdate(INiEnv env)
        {
            // If we got here and we have a pending update, it means it has
            // been installed and we can safely delete it.

            var pendingUpdateLocation = GetPendingUpdateLocation(env);

            if (Directory.Exists(pendingUpdateLocation))
                SEH.SinkExceptions(() => Directory.Delete(pendingUpdateLocation, true));
        }

        private static string GetPendingUpdateLocation(INiEnv env)
        {
            return Path.Combine(env.FileSystemRoot, "PendingUpdate");
        }

        private void CopyDirectory(string source, string target)
        {
            Directory.CreateDirectory(target);

            foreach (string path in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(path);

                File.Copy(Path.Combine(source, fileName), Path.Combine(target, fileName));
            }

            foreach (string path in Directory.GetDirectories(source))
            {
                string directoryName = Path.GetFileName(path);

                CopyDirectory(Path.Combine(source, directoryName), Path.Combine(target, directoryName));
            }
        }

        private static bool TryMoveTarget(string target, out string tempTarget)
        {
            tempTarget = null;

            if (!Directory.Exists(target))
                return true;

            // Generate a temporary name where we move the bin folder to.

            for (int i = 0;; i++)
            {
                tempTarget = target + "~" + i.ToString(CultureInfo.InvariantCulture);

                if (!Directory.Exists(tempTarget))
                    break;
            }

            // Try to take ownership of the bin folder. The previous instance
            // may still be shutting down, so we may have to try a few times
            // before we get ownership.

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    Directory.Move(target, tempTarget);

                    return true;
                }
                catch
                {
                    Thread.Sleep(TimeSpan.FromSeconds(0.5));
                }
            }

            return false;
        }

        private void ProcessPendingUpdates(INiEnv env)
        {
            var pendingUpdates = new List<PendingUpdate>();

            using (var key = Registry.CurrentUser.OpenSubKey(env.RegistryRoot + "\\InstalledProducts"))
            {
                foreach (string packageId in key.GetSubKeyNames())
                {
                    using (var packageKey = key.OpenSubKey(packageId))
                    {
                        bool uninstallPending = (packageKey.GetValue("UninstallPending") as int?).GetValueOrDefault() != 0;

                        if (uninstallPending)
                            pendingUpdates.Add(new PendingUpdate(packageId, PendingUpdateType.Uninstall));
                        else if (packageKey.GetValue("PendingVersion") != null)
                            pendingUpdates.Add(new PendingUpdate(packageId, PendingUpdateType.Update));
                    }
                }
            }

            if (pendingUpdates.Count > 0)
                PerformPendingUpdates(pendingUpdates);
        }

        private void PerformPendingUpdates(List<PendingUpdate> pendingUpdates)
        {
            using (var form = new UpdateForm(pendingUpdates))
            {
                form.ShowDialog(this);
            }
        }

        private void LoadPackages(INiEnv env)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(env.RegistryRoot + "\\InstalledProducts"))
            {
                foreach (string packageId in key.GetSubKeyNames())
                {
                    using (var packageKey = key.OpenSubKey(packageId))
                    {
                        bool installed = packageKey.GetValue("Version") != null;

                        if (!installed)
                        {
                            Log.WarnFormat("Skipping loading of package '{0}' because it is not installed", packageId);
                            continue;
                        }

                        bool enabled = (packageKey.GetValue("Disabled") as int?).GetValueOrDefault() == 0;

                        if (!enabled)
                        {
                            Log.WarnFormat("Skipping loading of package '{0}' because it is disabled", packageId);
                            continue;
                        }

                        LoadPackage(
                            env,
                            packageId,
                            Guid.Parse((string)packageKey.GetValue("Package"))
                        );
                    }
                }
            }

            var corePackage = _packages.SingleOrDefault(p => p.Guid == CorePackage);

            if (corePackage == null)
                throw new NetIdeException(Labels.CorePackageMissing);

            LoadRegistration(corePackage);

            foreach (var registration in new LoadOrderResolver(_packages).Resolve())
            {
                if (registration != corePackage)
                    LoadRegistration(registration);
            }

            ((NiCommandManager)GetService(typeof(INiCommandManager))).InitializeKeyboardMappings();
        }

        private void LoadRegistration(PackageRegistration registration)
        {
            registration.LoadPackage();
            _byPackage.Add(registration.Package, registration);
        }

        private void LoadPackage(INiEnv env, string packageId, Guid guid)
        {
            string packagePath = Path.Combine(
                env.FileSystemRoot,
                "Packages",
                packageId
            );

            if (Directory.Exists(packagePath))
                LoadPackageFromManifest(packagePath, packageId, guid);
            else
                Log.WarnFormat("Did not load package '{0}' because there was no package directory", packageId);
        }

        private void LoadPackageFromManifest(string packagePath, string packageId, Guid guid)
        {
            var manifest = GetManifest(packagePath);
            var metadata = GetMetadata(packagePath);

            if (!String.Equals(metadata.Id, packageId, StringComparison.OrdinalIgnoreCase))
                throw new NetIdeException(String.Format(Labels.InvalidPackageManifest, packagePath));

            // We provide the service container because this is
            // MarshalByRefObject, ServiceBase isn't.

            var serviceContainer = (IServiceContainer)GetService(typeof(IServiceContainer));

            _packages.Add(new PackageRegistration(serviceContainer, packagePath, metadata, manifest, guid));
        }

        private static PackageManifest GetManifest(string packagePath)
        {
            string fileName = Path.Combine(packagePath, PackageManifest.FileName);

            if (!File.Exists(fileName))
                return null;

            return Serialization.Deserialize<PackageManifest>(fileName);
        }

        private static PackageMetadata GetMetadata(string packagePath)
        {
            string[] fileNames = Directory.GetFiles(packagePath, "*.nuspec");

            if (fileNames.Length != 1)
                return null;

            return Serialization.Deserialize<NuSpec>(fileNames[0]).Metadata;
        }

        public bool QueryClose()
        {
            bool canClose = true;

            foreach (var package in _packages)
            {
                bool packageCanClose;

                ErrorUtil.ThrowOnFailure(package.Package.QueryClose(out packageCanClose));

                canClose = packageCanClose && canClose;
            }

            return canClose;
        }

        public PackageRegistration GetRegistration(INiPackage package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            return _byPackage[package];
        }

        private class PackageCollection : KeyedCollection<Guid, PackageRegistration>
        {
            protected override Guid GetKeyForItem(PackageRegistration item)
            {
                return item.Guid;
            }
        }
    }
}
