using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Services.CommandManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Xml.PackageManifest;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Services.PackageManager
{
    internal class PackageRegistration : IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _packagePath;
        private PackageDomain _domain;
        private bool _disposed;

        public string PackageId { get { return Metadata.Id; } }
        public Guid Guid { get; private set; }
        public INiPackage Package { get; private set; }
        public PackageMetadata Metadata { get; private set; }
        public PackageManifest Manifest { get; private set; }

        public PackageRegistration(IServiceProvider serviceProvider, string packagePath, PackageMetadata metadata, PackageManifest manifest, Guid guid)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");
            if (packagePath == null)
                throw new ArgumentNullException("packagePath");
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            if (manifest == null)
                throw new ArgumentNullException("manifest");

            Guid = guid;
            _serviceProvider = serviceProvider;
            _packagePath = packagePath;
            Metadata = metadata;
            Manifest = manifest;
        }

        public void LoadPackage()
        {
            try
            {
                Package = LoadPackageIsolated();
            }
            catch (Exception ex)
            {
                throw new NetIdeException(String.Format(Labels.PackageInitializationFailed, _packagePath), ex);
            }
        }

        public void InitializePackage()
        {
            try
            {
                ErrorUtil.ThrowOnFailure(Package.SetSite(_serviceProvider));

                IResource resource;
                ErrorUtil.ThrowOnFailure(Package.GetNiResources(out resource));

                if (resource != null)
                {
                    var commandManager = (NiCommandManager)_serviceProvider.GetService(typeof(INiCommandManager));

                    commandManager.LoadFromResources(Package, resource);
                }

                ErrorUtil.ThrowOnFailure(Package.Initialize());
            }
            catch (Exception ex)
            {
                throw new NetIdeException(String.Format(Labels.PackageInitializationFailed, _packagePath), ex);
            }
        }

        private INiPackage LoadPackageIsolated()
        {
            _domain = new PackageDomain(_packagePath);

            int pos = Manifest.EntryPoint.IndexOf(',');

            if (pos == -1)
                throw new NetIdeException(String.Format(NeutralResources.PackageEntryPointInvalid, Manifest.EntryPoint));

            string typeName = Manifest.EntryPoint.Substring(0, pos).Trim();
            string assemblyName = Manifest.EntryPoint.Substring(pos + 1).Trim();

            return _domain.CreatePackage(assemblyName, typeName);
        }

        public object CreateInstanceAndUnwrap(string type)
        {
            return _domain.CreateInstanceAndUnwrap(type);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_domain != null)
                {
                    _domain.Dispose();
                    _domain = null;
                }

                _disposed = true;
            }
        }
    }
}
