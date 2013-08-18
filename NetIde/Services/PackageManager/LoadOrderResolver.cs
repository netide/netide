using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Util;

namespace NetIde.Services.PackageManager
{
    internal class LoadOrderResolver
    {
        private readonly IKeyedCollection<Guid, PackageRegistration> _packages;
        private readonly Dictionary<string, PackageRegistration> _packagesById;
        private readonly Dictionary<PackageRegistration, bool> _seen;
        private List<PackageRegistration> _result;

        public LoadOrderResolver(IKeyedCollection<Guid, PackageRegistration> packages)
        {
            if (packages == null)
                throw new ArgumentNullException("packages");

            _packages = packages;

            _packagesById = _packages.ToDictionary(p => p.PackageId, p => p, StringComparer.OrdinalIgnoreCase);
            _seen = _packages.ToDictionary(p => p, p => false);
        }

        public IList<PackageRegistration> Resolve()
        {
            _result = new List<PackageRegistration>();

            foreach (var package in _packages)
            {
                AddPackageToLoadOrder(package);
            }

            return _result;
        }

        private void AddPackageToLoadOrder(PackageRegistration package)
        {
            if (_seen[package])
                return;

            _seen[package] = true;

            foreach (var dependency in package.Metadata.Dependencies)
            {
                PackageRegistration dependentPackage;

                if (_packagesById.TryGetValue(dependency.Id, out dependentPackage))
                    AddPackageToLoadOrder(dependentPackage);
            }

            _result.Add(package);
        }
    }
}
