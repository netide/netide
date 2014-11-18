using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Win32;
using NetIde.Update.NuGet;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Update
{
    public class NuGetQuerier
    {
        private const int PageCount = 25;

        public static PackageQueryResult Query(ContextName context, string nuGetSite, IList<string> packageIds, PackageStability stability)
        {
            return Query(context, nuGetSite, packageIds, stability, PackageQueryOrder.NameAscending, null);
        }

        public static PackageQueryResult Query(ContextName context, string nuGetSite, PackageStability stability, PackageQueryOrder queryOrder, int page)
        {
            return Query(context, nuGetSite, null, stability, queryOrder, page);
        }

        private static PackageQueryResult Query(ContextName context, string nuGetSite, IList<string> packageIds, PackageStability stability, PackageQueryOrder queryOrder, int? page)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (nuGetSite == null)
                throw new ArgumentNullException("nuGetSite");

            var service = new FeedContext_x0060_1(new Uri(nuGetSite));

            IQueryable<V2FeedPackage> query = service.Packages
                .IncludeTotalCount()
                .Where(p =>
                    p.IsAbsoluteLatestVersion &&
                    p.IsLatestVersion
                );

            if (packageIds != null)
            {
                if (packageIds.Count == 0)
                    throw new ArgumentOutOfRangeException("packageIds");

                // The feed doesn't support contains. Because of this, we're
                // building the ||'s of all ID's here dynamically.

                var idProperty = typeof(V2FeedPackage).GetProperty("Id");
                var parameterExpression = Expression.Parameter(typeof(V2FeedPackage), "p");

                Expression predicate = null;

                foreach (string id in packageIds)
                {
                    var thisPredicate = Expression.Equal(
                        Expression.Property(parameterExpression, idProperty),
                        Expression.Constant(id)
                    );

                    predicate =
                        predicate == null
                        ? thisPredicate
                        : Expression.OrElse(predicate, thisPredicate);
                }

                query = query.Where(Expression.Lambda<Func<V2FeedPackage, bool>>(
                    predicate, new[] { parameterExpression }
                ));
            }
            else
            {
                string prefix = context.Name + ".Package.";

                query = query.Where(p =>
                    p.Id.StartsWith(prefix) ||
                    p.Id.StartsWith("NetIde.Package.")
                );
            }

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

            if (page.HasValue)
            {
                if (page.Value > 0)
                    query = query.Skip(page.Value * PageCount);

                query = query.Take(PageCount);
            }

            var response = (QueryOperationResponse<V2FeedPackage>)((DataServiceQuery<V2FeedPackage>)query).Execute();

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                return new PackageQueryResult(
                    response.TotalCount,
                    page.GetValueOrDefault(0),
                    (int)((response.TotalCount / PageCount) + 1),
                    response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray()
                );
            }
        }

        public static PackageQueryResult GetUpdates(ContextName context, string nuGetSite, PackageStability stability, IEnumerable<IPackageId> ids)
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

                var packageMetadatas = response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray();

                return new PackageQueryResult(
                    GetTotalCount(response, packageMetadatas),
                    0,
                    1,
                    packageMetadatas
                );
            }
        }

        [DebuggerStepThrough]
        private static long GetTotalCount(QueryOperationResponse<V2FeedPackage> response, PackageMetadata[] packageMetadatas)
        {
            // TotalCount may be missing. When this is the case, we default
            // to the number of package meta's we got.

            try
            {
                return response.TotalCount;
            }
            catch (InvalidOperationException)
            {
                return packageMetadatas.Length;
            }
        }

        public static PackageMetadata ResolvePackageVersion(ContextName context, string nuGetSite, string packageId, string versionRestriction, PackageStability stability)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (nuGetSite == null)
                throw new ArgumentNullException("nuGetSite");
            if (packageId == null)
                throw new ArgumentNullException("packageId");
            if (versionRestriction == null)
                throw new ArgumentNullException("versionRestriction");

            string installedVersion = PackageManager.GetInstalledVersion(context, packageId);

            if (installedVersion == null)
            {
                // This method is used to resolve dependencies. If we don't
                // have the package installed, we check whether it's a valid
                // package ID at all. If not, the dependency probably is an
                // invalid dependency (or the NetIde.Runtime dependency)
                // and we completely ignore it.

                if (!PackageManager.IsValidPackageId(context, packageId))
                    return null;

                // We default to 0.0.0.0 for uninstalled packages. This could
                // give issues when the package available in NuGet has
                // version 0.0.0.0. The only way to use this API is to provide
                // a valid version number, so this currently is a limitation,
                // i.e. that you can't have NuGet packages of version 0.0.0.0.

                installedVersion = "0.0.0.0";
            }

            var service = new FeedContext_x0060_1(new Uri(nuGetSite));

            var query = service.CreateQuery<V2FeedPackage>("GetUpdates")
                .AddQueryOption("packageIds", "'" + packageId + "'")
                .AddQueryOption("versions", "'" + installedVersion + "'")
                .AddQueryOption("includePrerelease", (stability == PackageStability.IncludePrerelease) ? "true" : "false")
                .AddQueryOption("includeAllVersions", "false")
                .AddQueryOption("versionConstraints", "'" + versionRestriction + "'");

            var response = (QueryOperationResponse<V2FeedPackage>)query.Execute();

            PackageMetadata[] packages;

            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, context))
            {
                packages = response.Select(p => Deserialize(p, context, contextKey, nuGetSite)).ToArray();
            }

            Debug.Assert(packages.Length <= 1);

            if (packages.Length > 0)
                return packages[0];

            return null;
        }

        private static PackageMetadata Deserialize(V2FeedPackage package, ContextName context, RegistryKey contextKey, string nuGetSite)
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
    }
}
