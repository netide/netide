using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Update
{
    public class PackageQueryResult
    {
        public long TotalCount { get; private set; }
        public int Page { get; private set; }
        public int PageCount { get; private set; }
        public IList<PackageMetadata> Packages { get; private set; }

        public PackageQueryResult(long totalCount, int page, int pageCount, IList<PackageMetadata> packages)
        {
            if (packages == null)
                throw new ArgumentNullException("packages");

            TotalCount = totalCount;
            Page = page;
            PageCount = pageCount;
            Packages = packages;
        }
    }
}
