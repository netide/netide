using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Update.NuGet;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Update
{
    public class NuGetQuerier
    {
        private const int PageCount = 25;

        public static PackageQueryResult Query(string context, string nuGetSite, PackageStability stability, PackageQueryOrder queryOrder, int page)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (nuGetSite == null)
                throw new ArgumentNullException("nuGetSite");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                var service = new FeedContext_x0060_1(new Uri(nuGetSite));

                string prefix = context + ".Package.";

                IQueryable<V2FeedPackage> query = service.Packages
                    .IncludeTotalCount()
                    .Where(p =>
                        p.IsAbsoluteLatestVersion &&
                        p.IsLatestVersion && (
                            p.Id.StartsWith(prefix) ||
                            p.Id.StartsWith("NetIde.Package.")
                        )
                    );

                if (stability == PackageStability.StableOnly)
                    query = query.Where(p => !p.IsPrerelease);

                switch (queryOrder)
                {
                    case PackageQueryOrder.MostDownloads:
                        query = query.OrderByDescending(p => p.DownloadCount);
                        break;

                    case PackageQueryOrder.PublishedDate:
                        query = query.OrderByDescending(p => p.Published);
                        break;

                    case PackageQueryOrder.NameAscending:
                        query = query.OrderBy(p => p.Title);
                        break;

                    case PackageQueryOrder.NameDescending:
                        query = query.OrderByDescending(p => p.Title);
                        break;
                }

                if (page > 0)
                    query = query.Skip(page * PageCount);

                query = query.Take(PageCount);

                var response = (QueryOperationResponse<V2FeedPackage>)((DataServiceQuery<V2FeedPackage>)query).Execute();

                return new PackageQueryResult(
                    response.TotalCount,
                    page,
                    (int)((response.TotalCount / PageCount) + 1),
                    response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray()
                );
            }
        }

        public static PackageQueryResult Query(string context, string nuGetSite, PackageStability stability, IEnumerable<IPackageId> ids)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (nuGetSite == null)
                throw new ArgumentNullException("nuGetSite");
            if (ids == null)
                throw new ArgumentNullException("ids");

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                var service = new FeedContext_x0060_1(new Uri(nuGetSite));

                var query = service.CreateQuery<V2FeedPackage>("GetUpdates")
                    .AddQueryOption("packageIds", "'" + String.Join("|", ids.Select(p => p.Id)) + "'")
                    .AddQueryOption("versions", "'" + String.Join("|", ids.Select(p => p.Version)) + "'")
                    .AddQueryOption("includePrerelease", (stability == PackageStability.IncludePrerelease) ? "true" : "false")
                    .AddQueryOption("includeAllVersions", "false");

                var response = (QueryOperationResponse<V2FeedPackage>)query.Execute();

                return new PackageQueryResult(
                    response.TotalCount,
                    0,
                    1,
                    response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray()
                );
            }
        }

        private static PackageMetadata Deserialize(V2FeedPackage package, string context, RegistryKey contextKey, string nuGetSite)
        {
            return new PackageMetadata
            {
                Authors = package.Authors,
                Description = package.Description,
                DownloadCount = package.DownloadCount,
                GalleryDetailsUrl = package.GalleryDetailsUrl,
                IconUrl = package.IconUrl,
                Id = package.Id,
                Published = package.Published,
                Tags = package.Tags,
                Title = package.Title,
                Version = package.Version,
                NuGetSite = nuGetSite,
                Dependencies = ParseDependencies(package.Dependencies),
                State = PackageRegistry.GetPackageState(contextKey, context, package.Id)
            };
        }

        private static List<Dependency> ParseDependencies(string dependencies)
        {
            var result = new List<Dependency>();

            if (!String.IsNullOrEmpty(dependencies))
            {
                foreach (string dependency in dependencies.Split('|'))
                {
                    string[] parts = dependency.Split(':');

                    result.Add(new Dependency
                    {
                        Id = parts[0],
                        Version = parts[1]
                    });
                }
            }

            return result;
        }

        public static PackageMetadata ResolvePackageVersion(string context, string nuGetSite, string packageId, string versionRestriction, PackageStability stability)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (nuGetSite == null)
                throw new ArgumentNullException("nuGetSite");
            if (packageId == null)
                throw new ArgumentNullException("packageId");
            if (versionRestriction == null)
                throw new ArgumentNullException("versionRestriction");

            string installedVersion = null;

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            using (var packageKey = contextKey.OpenSubKey("InstalledProducts\\" + packageId))
            {
                // This method is used to resolve dependencies. If we don't
                // have the package installed, we check whether it's a valid
                // package ID at all. If not, the dependency probably is an
                // invalid dependency (or the NetIde.Runtime dependency)
                // and we completely ignore it.

                if (packageKey == null)
                {
                    if (!PackageManager.IsValidPackageId(context, packageId))
                        return null;
                }
                else
                {
                    installedVersion = (string)packageKey.GetValue("Version");
                }

                // We default to 0.0.0.0 for uninstalled packages. This could
                // give issues when the package available in NuGet has
                // version 0.0.0.0. The only way to use this API is to provide
                // a valid version number, so this currently is a limitation,
                // i.e. that you can't have NuGet packages of version 0.0.0.0.

                if (installedVersion == null)
                    installedVersion = "0.0.0.0";
            }

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                var service = new FeedContext_x0060_1(new Uri(nuGetSite));

                var query = service.CreateQuery<V2FeedPackage>("GetUpdates")
                    .AddQueryOption("packageIds", "'" + packageId + "'")
                    .AddQueryOption("versions", "'" + installedVersion + "'")
                    .AddQueryOption("includePrerelease", (stability == PackageStability.IncludePrerelease) ? "true" : "false")
                    .AddQueryOption("includeAllVersions", "false")
                    .AddQueryOption("versionConstraints", "'" + versionRestriction + "'");

                var response = (QueryOperationResponse<V2FeedPackage>)query.Execute();

                var packages = response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray();

                Debug.Assert(packages.Length <= 1);

                if (packages.Length > 0)
                    return packages[0];

                return null;
            }
        }
    }
}
