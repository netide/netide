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
using NetIde.Update;
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
        private readonly Dictionary<int, Notification> _notifications = new Dictionary<int, Notification>();

        private static readonly Guid CorePackage = new Guid("129646DC-01E6-4443-ABAC-453F731EA7F5");
        private INiNotificationManager _notificationManager;
        private INiEnv _env;

        public IKeyedCollection<Guid, PackageRegistration> Packages { get; private set; }

        public NiPackageManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Packages = ReadOnlyKeyedCollection.Create(_packages);
        }

        public void Initialize()
        {
            _env = (INiEnv)GetService(typeof(INiEnv));

            ProcessRuntimeUpdate();
            RemovePendingUpdate();
            ProcessPendingUpdates();
            LoadPackages();
            FindUpdates();
        }

        private void ProcessRuntimeUpdate()
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

            var pendingUpdateLocation = GetPendingUpdateLocation();

            // And we want to copy into the bin folder.

            string target = Path.Combine(_env.FileSystemRoot, "bin");

            // Move the current target directory out of the way.

            string tempTarget;
            if (!TryMoveTarget(target, out tempTarget))
            {
                // If we aren't able to move the target out of the way
                // (i.e. can't take ownership of the directory), we can't
                // continue. We got here in the first place because of a
                // pending update to NetIde.Package.Core, so we cancel
                // this and just restart.

                PackageUpdater.CancelCorePackageUpdate(_env);
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
                WorkingDirectory = _env.FileSystemRoot,
                UseShellExecute = false
            });

            Environment.Exit(0);
        }

        private void RemovePendingUpdate()
        {
            // If we got here and we have a pending update, it means it has
            // been installed and we can safely delete it.

            var pendingUpdateLocation = GetPendingUpdateLocation();

            if (Directory.Exists(pendingUpdateLocation))
                SEH.SinkExceptions(() => Directory.Delete(pendingUpdateLocation, true));
        }

        private string GetPendingUpdateLocation()
        {
            return Path.Combine(_env.FileSystemRoot, "PendingUpdate");
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

            for (int i = 0; ; i++)
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

        private void ProcessPendingUpdates()
        {
            var pendingUpdates = new List<PendingUpdate>();

            using (var key = Registry.CurrentUser.OpenSubKey(_env.RegistryRoot + "\\InstalledProducts"))
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

        private void LoadPackages()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(_env.RegistryRoot + "\\InstalledProducts"))
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
            registration.InitializePackage();
        }

        private void LoadPackage(string packageId, Guid guid)
        {
            string packagePath = Path.Combine(
                _env.FileSystemRoot,
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

        public HResult OpenPackageManager()
        {
            try
            {
                object result;
                return ((INiCommandManager)GetService(typeof(INiCommandManager))).Exec(
                    NiResources.Tools_PackageManagement,
                    null,
                    out result
                );
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void FindUpdates()
        {
            _notificationManager = (INiNotificationManager)GetService(typeof(INiNotificationManager));

            new NotificationListener(this);

            var synchronizationContext = SynchronizationContext.Current;

            ThreadPool.QueueUserWorkItem(p => FindUpdates(synchronizationContext));
        }

        private void FindUpdates(SynchronizationContext synchronizationContext)
        {
            try
            {
                var context = new ContextName(_env.ContextName, _env.Experimental);
                var installedPackages = PackageRegistry.GetInstalledPackages(context);

                var packages = NuGetQuerier.GetUpdates(
                    context,
                    _env.NuGetSite,
                    PackageStability.StableOnly,
                    installedPackages.Packages
                );

                synchronizationContext.Post(
                    p => CreateNotifications(packages),
                    null
                );
            }
            catch (Exception ex)
            {
                Log.Warn("Could not find updates", ex);
            }
        }

        private void CreateNotifications(PackageQueryResult updates)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(_env.RegistryRoot + "\\PackageManager"))
            {
                foreach (var update in updates.Packages)
                {
                    using (var packageKey = key.CreateSubKey(update.Id))
                    {
                        // Was this update dismissed already?

                        string lastDismissed = (string)packageKey.GetValue("LastDismissedUpdate");
                        if (lastDismissed == update.Version)
                            continue;

                        // Have we already reported this one?

                        DateTime reportedDate = DateTime.Now;

                        string lastReported = (string)packageKey.GetValue("LastReportedUpdate");
                        if (lastReported != null)
                        {
                            string[] parts = lastReported.Split(',').Select(p => p.Trim()).ToArray();
                            if (parts.Length == 2)
                            {
                                string lastReportedVersion = parts[0];

                                if (lastReportedVersion == update.Version)
                                {
                                    DateTime lastReportedDate;
                                    if (DateTime.TryParseExact(parts[1], "o", CultureInfo.InvariantCulture, DateTimeStyles.None, out lastReportedDate))
                                        reportedDate = lastReportedDate;
                                }
                            }
                        }

                        // Update the last reported update.

                        packageKey.SetValue("LastReportedUpdate", update.Version + ", " + reportedDate.ToString("o"));

                        // Create the notification.

                        int cookie;
                        ErrorUtil.ThrowOnFailure(_notificationManager.AddItem(
                            new NiNotificationItem
                            {
                                Subject = String.Format(Labels.UpdateAvailable, update.Title),
                                Message = update.Description,
                                Created = reportedDate,
                                Priority = NiNotificationItemPriority.Informational
                            },
                            out cookie
                        ));

                        _notifications.Add(cookie, new Notification(update.Id, update.Version));
                    }
                }
            }
        }

        public HResult GetInstallationPath(INiPackage package, out string path)
        {
            path = null;

            try
            {
                if (package == null)
                    throw new ArgumentNullException("package");

                PackageRegistration registration;
                if (!_byPackage.TryGetValue(package, out registration))
                    return HResult.False;

                path = Path.Combine(
                    _env.FileSystemRoot,
                    "Packages",
                    registration.PackageId
                );

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private class PackageCollection : KeyedCollection<Guid, PackageRegistration>
        {
            protected override Guid GetKeyForItem(PackageRegistration item)
            {
                return item.Guid;
            }
        }

        private class Notification
        {
            public string Id { get; private set; }
            public string Version { get; private set; }

            public Notification(string id, string version)
            {
                Id = id;
                Version = version;
            }
        }

        private class NotificationListener : NiEventSink, INiNotificationManagerNotify
        {
            private static readonly ILog Log = LogManager.GetLogger(typeof(NotificationListener));

            private readonly NiPackageManager _packageManager;

            public NotificationListener(NiPackageManager packageManager)
                : base(packageManager._notificationManager)
            {
                _packageManager = packageManager;
            }

            public void OnClicked(int cookie)
            {
                try
                {
                    if (_packageManager._notifications.ContainsKey(cookie))
                        ErrorUtil.ThrowOnFailure(_packageManager.OpenPackageManager());
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to open package manager", ex);
                }
            }

            public void OnDismissed(int cookie)
            {
                try
                {
                    Notification notification;
                    if (!_packageManager._notifications.TryGetValue(cookie, out notification))
                        return;

                    using (var key = Registry.CurrentUser.CreateSubKey(_packageManager._env.RegistryRoot + "\\PackageManager\\" + notification.Id))
                    {
                        key.SetValue("LastDismissedUpdate", notification.Version);
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn("Failed to dismiss package manager notification", ex);
                }
            }
        }
    }
}
