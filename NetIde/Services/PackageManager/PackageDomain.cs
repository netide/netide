using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.PackageManager
{
    internal class PackageDomain : IDisposable
    {
        private bool _disposed;
        private AppDomain _appDomain;

        public PackageDomain(string packagePath)
        {
            if (packagePath == null)
                throw new ArgumentNullException("packagePath");

            var setup = new AppDomainSetup
            {
                ApplicationName = "Net IDE Plugin",
                ApplicationBase = packagePath
            };

            _appDomain = AppDomain.CreateDomain(
                setup.ApplicationName,
                AppDomain.CurrentDomain.Evidence,
                setup
            );
        }

        public INiPackage CreatePackage(string assemblyName, string typeName)
        {
            return (INiPackage)_appDomain.CreateInstanceAndUnwrap(
                assemblyName,
                typeName
            );
        }

        public object CreateInstanceAndUnwrap(string type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            string[] parts = type.Split(',');

            if (parts.Length < 2)
                throw new ArgumentOutOfRangeException("type");

            return _appDomain.CreateInstanceAndUnwrap(parts[1], parts[0]);
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
