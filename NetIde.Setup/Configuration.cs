using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Setup.Pages;
using NetIde.Update;

namespace NetIde.Setup
{
    public class Configuration
    {
        public ContextName Context { get; private set; }

        public string Title { get; private set; }

        public string NuGetWebsite { get; private set; }

        public string License { get; private set; }

        public string StartMenu { get; private set; }

        public string InstallationPath { get; private set; }

        public IList<string> Packages { get; private set; }

        public Configuration()
        {
            string basePath = Path.GetDirectoryName(GetType().Assembly.Location);

            string licenseFileName = Path.Combine(basePath, "license.txt");

            if (File.Exists(licenseFileName))
                License = licenseFileName;

            string fileName = Path.Combine(basePath, "config.ini");

            foreach (string line in File.ReadAllLines(fileName))
            {
                string trimmed = line.Trim();

                if (trimmed.Length == 0 || trimmed[0] == '#')
                    continue;

                string[] parts = trimmed.Split(new[] { '=' }, 2);

                if (parts.Length == 2)
                {
                    switch (parts[0])
                    {
                        case "Context": Context = new ContextName(parts[1]); break;
                        case "Title": Title = parts[1]; break;
                        case "NuGetWebsite": NuGetWebsite = parts[1]; break;
                        case "Packages": Packages = new ReadOnlyCollection<string>(parts[1].Split(';')); break;
                        case "StartMenu": StartMenu = parts[1]; break;
                    }
                }
            }

            if (
                Context == null ||
                Title == null ||
                NuGetWebsite == null ||
                Packages == null || Packages.Count == 0
            )
                throw new InvalidOperationException("Invalid configuration");

            using (var key = PackageRegistry.OpenRegistryRoot(false, Context))
            {
                if (key != null)
                    InstallationPath = (string)key.GetValue("InstallationPath");
            }
        }

        public bool ShowPage(Page page)
        {
            switch (page)
            {
                case Page.License: return License != null;
                case Page.InstallationPath:
                case Page.StartMenu:
                    return InstallationPath == null;

                default:
                    return true;
            }
        }
    }
}
