using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using NetIde.Services.Env;
using NetIde.Shell.Interop;
using NetIde.Update;

namespace NetIde.Services.PackageManager
{
    internal class PackageUpdater
    {
        public const string CorePackageId = "NetIde.Package.Core";
        public const string RuntimePackageId = "NetIde.Runtime";

        private readonly List<PendingUpdate> _pendingUpdates;
        private readonly NiEnv _env;

        public event ProgressEventHandler ProgressChanged;

        protected virtual void OnProgressChanged(ProgressEventArgs e)
        {
            var ev = ProgressChanged;
            if (ev != null)
                ev(this, e);
        }

        public event ExceptionEventHandler Completed;

        protected virtual void OnCompleted(ExceptionEventArgs e)
        {
            var ev = Completed;
            if (ev != null)
                ev(this, e);
        }

        public PackageUpdater(IServiceProvider serviceProvider, IEnumerable<PendingUpdate> pendingUpdates)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (pendingUpdates == null)
                throw new ArgumentNullException("pendingUpdates");

            _pendingUpdates = pendingUpdates.ToList();
            _env = (NiEnv)serviceProvider.GetService(typeof(INiEnv));
        }

        public void Start()
        {
            var thread = new Thread(ThreadProc);

            thread.Start();
        }

        private void ThreadProc()
        {
            Exception exception = null;

            try
            {
                // We sort the packages. Removals come before updates because
                // we want to get these out of the way first. Then comes
                // the update to NetIde.Package.Core. We do this first because
                // it is treated special. After that come the updates to the
                // rest of the packages.

                _pendingUpdates.Sort(ComparePendingUpdate);

                for (int i = 0; i < _pendingUpdates.Count; i++)
                {
                    ProcessUpdate(i);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                OnCompleted(new ExceptionEventArgs(exception));
            }
        }

        private int ComparePendingUpdate(PendingUpdate x, PendingUpdate y)
        {
            if (x.Type != y.Type)
                return x.Type == PendingUpdateType.Uninstall ? -1 : 1;

            bool xIsCore = x.PackageId == CorePackageId;
            bool yIsCore = y.PackageId == CorePackageId;

            if (xIsCore != yIsCore)
                return xIsCore ? -1 : 1;

            return String.Compare(x.PackageId, y.PackageId, StringComparison.OrdinalIgnoreCase);
        }

        private void ProcessUpdate(int index)
        {
            var update = _pendingUpdates[index];

            using (var packageKey = Registry.CurrentUser.OpenSubKey(_env.RegistryRoot + "\\InstalledProducts\\" + update.PackageId))
            {
                switch (update.Type)
                {
                    case PendingUpdateType.Uninstall:
                        PerformUninstall(index);
                        break;

                    case PendingUpdateType.Update:
                        if (ShouldUpdateRuntime(update, packageKey))
                            PerformRuntimeUpdate(index, packageKey);
                        else
                            PerformUpdate(index, packageKey);
                        break;

                    default:
                        Debug.Fail("Unexpected package update type");
                        return;
                }
            }
        }

        private static bool ShouldUpdateRuntime(PendingUpdate update, RegistryKey packageKey)
        {
            if (
                update.PackageId != CorePackageId ||
                update.Type != PendingUpdateType.Update
            )
                return false;

            Version version;
            if (!Version.TryParse((string)packageKey.GetValue("PendingVersion"), out version))
                return true;

            // We check whether we need to update the runtime by checking whether
            // the assembly version of the main assembly (this) equals the
            // expected version of the NetIde.Package.Core package.

            return version != typeof(PackageUpdater).Assembly.GetName().Version;
        }

        private void PerformRuntimeUpdate(int index, RegistryKey packageKey)
        {
            // A runtime update involves updating two packages. The core package
            // is of course updated, as part of the runtime update, we also
            // update the NetIde.Runtime package. This package contains the
            // main executable and is used to overwrite the bin directory
            // of the installation.

            OnProgressChanged(new ProgressEventArgs(
                String.Format(Labels.DownloadingPackage, RuntimePackageId),
                index,
                _pendingUpdates.Count
            ));

            string tempFileName = Path.GetTempFileName();

            try
            {
                DownloadPackage(packageKey, RuntimePackageId, tempFileName);

                new PackageRuntimeInstaller(_env.Context, tempFileName).Execute();
            }
            catch
            {
                // Cancel the core update to prevent from getting into an
                // endless loop.

                CancelCorePackageUpdate(_env);

                throw;
            }
            finally
            {
                SEH.SinkExceptions(() => File.Delete(tempFileName));
            }

            // The runtime installer restarted the application with a request
            // to update the runtime. We can exit.

            Environment.Exit(0);
        }

        private void DownloadPackage(RegistryKey packageKey, string packageId, string targetFileName)
        {
            string nuGetSite = (string)packageKey.GetValue("NuGetSite");
            string pendingVersion = (string)packageKey.GetValue("PendingVersion");

            string url = String.Format(
                "{0}/package/{1}/{2}",
                nuGetSite.TrimEnd('/'),
                packageId,
                pendingVersion
            );

            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, targetFileName);
            }
        }

        private void PerformUpdate(int index, RegistryKey packageKey)
        {
            var update = _pendingUpdates[index];

            OnProgressChanged(new ProgressEventArgs(
                String.Format(Labels.DownloadingPackage, update.PackageId),
                index,
                _pendingUpdates.Count
            ));

            string tempFileName = Path.GetTempFileName();

            try
            {
                DownloadPackage(packageKey, update.PackageId, tempFileName);

                OnProgressChanged(new ProgressEventArgs(
                    packageKey.GetValue("Version") != null
                        ? String.Format(Labels.UpdatingPackage, update.PackageId)
                        : String.Format(Labels.InstallingPackage, update.PackageId),
                    index,
                    _pendingUpdates.Count
                ));

                new PackageInstaller(_env.Context, tempFileName).Execute();
            }
            finally
            {
                SEH.SinkExceptions(() => File.Delete(tempFileName));
            }
        }

        private void PerformUninstall(int index)
        {
            var update = _pendingUpdates[index];

            OnProgressChanged(new ProgressEventArgs(
                String.Format(Labels.UninstallingPackage, update.PackageId),
                index,
                _pendingUpdates.Count
            ));

            new PackageUninstaller(_env.Context, update.PackageId).Execute();
        }

        public static void CancelCorePackageUpdate(INiEnv env)
        {
            using (var key = Registry.CurrentUser.CreateSubKey(env.RegistryRoot + "\\InstalledProducts\\" + CorePackageId))
            {
                Debug.Assert(key != null);

                if (key != null)
                    key.DeleteValue("PendingVersion");
            }
        }
    }
}
