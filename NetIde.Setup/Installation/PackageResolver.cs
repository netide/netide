using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NetIde.Update;

namespace NetIde.Setup.Installation
{
    public class PackageResolver
    {
        public IList<SetupPackage> ResolvePackages(Configuration configuration)
        {
            // Check the version of NetIde.Runtime (this is the version of the
            // main executable).

            string installedRuntimeVersion = GetInstalledRuntimeVersion(configuration.Context);
            
            // Start with loading the packages that were in the configuration.

            var query = NuGetQuerier.Query(
                configuration.Context,
                configuration.NuGetWebsite,
                configuration.Packages,
                PackageStability.StableOnly
            );

            var packages = query.Packages.ToDictionary(
                p => p.Id,
                p => new SetupPackage(p, true, GetInstalledVersion(configuration.Context, p.Id, installedRuntimeVersion)),
                StringComparer.OrdinalIgnoreCase
            );

            if (packages.Count != configuration.Packages.Count)
            {
                var missingPackages = configuration.Packages.Where(p => !packages.ContainsKey(p)).ToArray();

                throw new InvalidOperationException(String.Format(
                    Labels.CannotDownloadPackageMetadata,
                    String.Join("', '", missingPackages)
                ));
            }

            // Now, load all dependencies.

            while (true)
            {
                bool hadOne = false;

                foreach (var dependency in packages.SelectMany(p => p.Value.Metadata.Dependencies).ToArray())
                {
                    if (packages.ContainsKey(dependency.Id))
                        continue;

                    hadOne = true;

                    var package = NuGetQuerier.ResolvePackageVersion(
                        configuration.Context,
                        configuration.NuGetWebsite,
                        dependency.Id,
                        dependency.Version,
                        PackageStability.StableOnly
                    );

                    // If we didn't get a package, it's either because it's
                    // already up to date, or because it's an invalid package.
                    // Do a normal query to see whether the package exists at all.

                    if (package == null)
                    {
                        query = NuGetQuerier.Query(
                            configuration.Context,
                            configuration.NuGetWebsite,
                            new[] { dependency.Id },
                            PackageStability.StableOnly
                        );

                        package = query.Packages.SingleOrDefault();
                    }

                    if (package == null)
                    {
                        throw new InvalidOperationException(String.Format(
                            Labels.CannotDownloadPackageMetadata,
                            dependency.Id
                        ));
                    }

                    packages.Add(
                        package.Id,
                        new SetupPackage(
                            package,
                            false,
                            GetInstalledVersion(configuration.Context, package.Id, installedRuntimeVersion)
                        )
                    );
                }

                if (!hadOne)
                    break;
            }

            // Determine the load order.

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var result = new List<SetupPackage>();

            while (true)
            {
                bool hadOne = false;

                foreach (var package in packages.Values)
                {
                    if (
                        !seen.Contains(package.Metadata.Id) &&
                        package.Metadata.Dependencies.All(p => seen.Contains(p.Id))
                    )
                    {
                        hadOne = true;

                        seen.Add(package.Metadata.Id);
                        result.Add(package);
                    }
                }

                if (!hadOne)
                    break;
            }

            return result;
        }

        private string GetInstalledVersion(string context, string packageId, string installedRuntimeVersion)
        {
            if (String.Equals(packageId, PackageManager.RuntimePackageId, StringComparison.OrdinalIgnoreCase))
                return installedRuntimeVersion;

            return PackageManager.GetInstalledVersion(context, packageId);
        }

        private string GetInstalledRuntimeVersion(string context)
        {
            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                if (contextKey == null)
                    return null;

                string installationPath = (string)contextKey.GetValue("InstallationPath");

                return VersionResolver.GetVersion(
                    PackageManager.GetEntryAssemblyLocation(installationPath)
                );
            }
        }

        private class VersionResolver : MarshalByRefObject
        {
            public static string GetVersion(string fileName)
            {
                if (!File.Exists(fileName))
                    return null;

                var setup = new AppDomainSetup
                {
                    ApplicationBase = Path.GetDirectoryName(typeof(VersionResolver).Assembly.Location),
                    ApplicationName = "Version Resolver"
                };

                var appDomain = AppDomain.CreateDomain(
                    setup.ApplicationName,
                    AppDomain.CurrentDomain.Evidence,
                    setup
                );

                try
                {
                    var resolver = (VersionResolver)appDomain.CreateInstanceAndUnwrap(
                        typeof(VersionResolver).Assembly.FullName,
                        typeof(VersionResolver).FullName,
                        false /* ignoreCase */,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                        null,
                        null,
                        null,
                        null
                    );

                    return resolver.Resolve(fileName);
                }
                finally
                {
                    AppDomain.Unload(appDomain);
                }
            }

            private string Resolve(string fileName)
            {
                return Assembly.ReflectionOnlyLoadFrom(fileName).GetName().Version.ToString();
            }
        }
    }
}
