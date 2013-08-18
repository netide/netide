using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using NetIde.Shell.Interop;

namespace NetIde.Update
{
    public abstract class PackageManager
    {
        public static bool IsValidPackageId(string context, string packageId)
        {
            return
                packageId.StartsWith("NetIde.Package.", StringComparison.OrdinalIgnoreCase) ||
                packageId.StartsWith(context + ".Package.", StringComparison.OrdinalIgnoreCase);
        }

        public string Context { get; private set; }

        protected PackageManager(string context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Context = context;

            if (Context.IndexOfAny(new[] { '/', '\\' }) != -1)
                throw new PackageInstallationException(Labels.ContextNameInvalid, 1);
        }

        public abstract void Execute();

        protected bool TryParseEntryPoint(string entryPoint, out string entryAssembly, out string entryTypeName)
        {
            entryAssembly = null;
            entryTypeName = null;

            int pos = entryPoint.IndexOf(',');

            if (pos == -1)
                return false;

            entryTypeName = entryPoint.Substring(0, pos).Trim();
            entryAssembly = entryPoint.Substring(pos + 1).Trim();

            return true;
        }

        protected IDisposable CreateAppDomain(string packagePath, string entryPoint, out INiPackage package)
        {
            string entryAssembly;
            string entryTypeName;

            if (!TryParseEntryPoint(entryPoint, out entryAssembly, out entryTypeName))
                throw new PackageInstallationException(Labels.InvalidManifest, 3);

            var setup = new AppDomainSetup
            {
                ApplicationBase = packagePath,
                ApplicationName = "Net IDE package manager"
            };

            var appDomain = AppDomain.CreateDomain(
                setup.ApplicationName,
                AppDomain.CurrentDomain.Evidence,
                setup
            );

            try
            {
                package = (INiPackage)appDomain.CreateInstanceAndUnwrap(
                    entryAssembly,
                    entryTypeName
                );

                return new AppDomainUnloader(appDomain);
            }
            catch
            {
                AppDomain.Unload(appDomain);

                throw;
            }
        }

        protected void ExtractPackage(string packageFileName, string target)
        {
            using (var zipFile = new ZipFile(packageFileName))
            {
                const string toolsPrefix = "Tools/";

                foreach (ZipEntry entry in zipFile)
                {
                    string fileName = entry.Name;

                    if (fileName.StartsWith(toolsPrefix, StringComparison.OrdinalIgnoreCase))
                        fileName = fileName.Substring(toolsPrefix.Length);
                    else if (!fileName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase))
                        continue;

                    fileName = Path.Combine(target, fileName.Replace('/', Path.DirectorySeparatorChar));

                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));

                    using (var source = zipFile.GetInputStream(entry))
                    using (var destination = File.Create(fileName))
                    {
                        source.CopyTo(destination);
                    }
                }
            }
        }

        protected string GetFileSystemRoot()
        {
            using (var contextKey = OpenContextRegistry(false))
            {
                if (contextKey != null)
                    return contextKey.GetValue("InstallationPath") as string;
                else
                    throw new PackageInstallationException(Labels.ContextDoesNotExist, 2);
            }
        }

        protected RegistryKey OpenContextRegistry(bool writable)
        {
            return PackageRegistry.OpenRegistryRoot(writable, Context);
        }

        private class AppDomainUnloader : IDisposable
        {
            private AppDomain _appDomain;
            private bool _disposed;

            public AppDomainUnloader(AppDomain appDomain)
            {
                _appDomain = appDomain;
            }

            public void Dispose()
            {
                if (!_disposed)
                {
                    if (_appDomain != null)
                    {
                        AppDomain.Unload(_appDomain);
                        _appDomain = null;
                    }

                    _disposed = true;
                }
            }
        }
    }
}
