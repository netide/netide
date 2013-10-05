using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NetIde.Xml;
using NetIde.Xml.BuildConfiguration;
using NuGet;

namespace NetIde.BuildTasks
{
    public partial class NetIdeTask : Task
    {
        [Required]
        public string Configuration { get; set; }

        [Required]
        public string SolutionDir { get; set; }

        [Required]
        public string ProjectDir { get; set; }

        [Required]
        public string TargetDir { get; set; }

        [Required]
        public string TargetPath { get; set; }

        public override bool Execute()
        {
            if (!File.Exists(Configuration))
                return true;

            var configuration = LoadConfiguration();

            ExecuteCreateContext(configuration);
            ExecuteBuildNuGetPackages(configuration);
            ExecuteInstallPackages(configuration);

            return true;
        }

        private BuildConfiguration LoadConfiguration()
        {
            string resourceName = GetType().Namespace + ".BuildConfiguration.xsd";

            using (var schema = GetType().Assembly.GetManifestResourceStream(resourceName))
            {
                XmlValidation.Validate(Configuration, schema, Ns.BuildConfiguration);
            }

            return Serialization.DeserializeXml<BuildConfiguration>(
                File.ReadAllText(Configuration)
            );
        }

        private string TranslatePath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            path = Regex.Replace(path, @"\$\((.*?)\)", p =>
            {
                switch (p.Groups[1].Value)
                {
                    case "ProjectDir": return ProjectDir;
                    case "TargetDir": return TargetDir;
                    case "TargetPath": return TargetPath;
                    case "SolutionDir": return SolutionDir;
                    default: throw new InvalidOperationException(String.Format(Labels.ReplacementVariableMissing, p.Groups[1].Value));
                }
            });

            if (path.IndexOf('*') != -1)
            {
                var matches = PathResolver.PerformWildcardSearch(
                    Environment.CurrentDirectory,
                    FlattenPath(path)
                ).ToArray();

                if (matches.Length == 0)
                    throw new InvalidOperationException(String.Format(Labels.CannotResolvePath, path));
                if (matches.Length > 1)
                    throw new InvalidOperationException(String.Format(Labels.MultiplePathsMatched, path));

                path = matches[0];
            }

            return path;
        }

        private string FlattenPath(string path)
        {
            // NuGet PathResolver doesn't like duplicate \'s etc.

            var parts = new List<string>();

            foreach (string part in path.Split(Path.DirectorySeparatorChar))
            {
                switch (part)
                {
                    case "":
                    case ".":
                        continue;

                    case "..":
                        parts.RemoveAt(parts.Count - 1);
                        break;

                    default:
                        parts.Add(part);
                        break;
                }
            }

            return String.Join(new string(Path.DirectorySeparatorChar, 1), parts);
        }
    }
}
