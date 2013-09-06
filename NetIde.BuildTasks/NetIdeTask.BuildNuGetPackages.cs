using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Xml.BuildConfiguration;
using NuGet;

namespace NetIde.BuildTasks
{
    partial class NetIdeTask
    {
        private void ExecuteBuildNuGetPackages(BuildConfiguration configuration)
        {
            foreach (var buildNuGetPackage in configuration.BuildNuGetPackages)
            {
                try
                {
                    var properties = new PropertyProvider();

                    string version = buildNuGetPackage.Version;

                    if (version == null)
                        version = VersionRetriever.GetVersion(TargetPath);

                    properties.Properties["version"] = version;

                    string manifest = TranslatePath(buildNuGetPackage.Manifest);

                    var builder = new PackageBuilder(
                        manifest,
                        buildNuGetPackage.BasePath ?? TargetDir,
                        properties,
                        false
                    );

                    string target =
                        buildNuGetPackage.Target != null
                        ? TranslatePath(buildNuGetPackage.Target)
                        : Path.Combine(TargetDir, Path.GetFileNameWithoutExtension(manifest) + "." + version + ".nupkg");

                    bool isExistingPackage = File.Exists(target);
                    try
                    {
                        using (Stream stream = File.Create(target))
                        {
                            builder.Save(stream);
                        }
                    }
                    catch
                    {
                        if (!isExistingPackage && File.Exists(target))
                            File.Delete(target);

                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Log.LogErrorFromException(ex);
                }
            }
        }

        private class PropertyProvider : IPropertyProvider
        {
            public IDictionary<string, object> Properties { get; private set; }

            public PropertyProvider()
            {
                Properties = new Dictionary<string, object>();
            }

            public dynamic GetPropertyValue(string propertyName)
            {
                object value;
                if (Properties.TryGetValue(propertyName, out value))
                    return value;

                return null;
            }
        }
    }
}
