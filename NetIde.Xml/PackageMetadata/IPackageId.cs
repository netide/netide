using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Xml.PackageMetadata
{
    public interface IPackageId
    {
        string Id { get; }
        string Version { get; }
    }
}
