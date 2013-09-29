using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using NetIde.Setup.Support;
using NetIde.Update;
using NetIde.Xml;
using NetIde.Xml.Context;
using NetIde.Xml.PackageMetadata;

namespace NetIde.Setup.Installation
{
    partial class PackageInstaller
    {
        private Dictionary<PackageMetadata, string> _downloads;

        private void CreateContext()
        {
            if (Program.Configuration.InstallationPath != null)
                return;

            AddLog(Labels.CreatingContext, Program.Configuration.Context);

            // Ensure we have the installation folder.

            if (!Directory.Exists(_configuration.InstallationPath))
                Directory.CreateDirectory(_configuration.InstallationPath);

            // Create the registry root.

            using (var key = PackageRegistry.OpenRegistryRoot(true, Program.Configuration.Context))
            {
                key.SetValue("InstallationPath", _configuration.InstallationPath);
            }

            // Write the NiContext.xml file.

            var document = new XDocument(
                new XElement(
                    (XNamespace)Ns.Context + "context",
                    new XAttribute(
                        "name",
                        Program.Configuration.Context
                    ),
                    new XAttribute(
                        "nuGetSite",
                        Program.Configuration.NuGetWebsite
                    )
                )
            );

            document.Save(Path.Combine(_configuration.InstallationPath, Context.FileName));
        }

        private void DownloadPackages()
        {
            _downloads = new Dictionary<PackageMetadata, string>();

            using (var webClient = new WebClient())
            {
                for (int i = 0; i < _packages.Length; i++)
                {
                    var package = _packages[i];

                    AddLog(Labels.DownloadingPackage, package.Id);
                    SetProgress((i + 1) * 0.5 / _packages.Length);

                    string url = String.Format(
                        "{0}/package/{1}/{2}",
                        Program.Configuration.NuGetWebsite.TrimEnd('/'),
                        package.Id,
                        package.Version
                    );

                    string targetFileName = Path.GetTempFileName();

                    webClient.DownloadFile(url, targetFileName);

                    _downloads.Add(package, targetFileName);
                }
            }
        }

        private void InstallPackages()
        {
            for (int i = 0; i < _packages.Length; i++)
            {
                var package = _packages[i];

                AddLog(Labels.InstallingPackage, package.Id);
                SetProgress((_packages.Length + i + 1) * 0.5 / _packages.Length);

                if (String.Equals(package.Id, PackageManager.RuntimePackageId, StringComparison.OrdinalIgnoreCase))
                {
                    new Update.PackageRuntimeInstaller(
                        Program.Configuration.Context,
                        _downloads[package],
                        true /* inPlace */
                    ).Execute();
                }
                else
                {
                    new Update.PackageInstaller(
                        Program.Configuration.Context,
                        _downloads[package]
                    ).Execute();
                }
            }
        }

        private void CreateShortcuts()
        {
            string mainIcon = FindMainIcon();

            if (_configuration.CreateStartMenuShortcut)
            {
                AddLog(Labels.CreatingStartMenuShortcut);

                CreateShortcut(
                    Path.Combine(
                        NativeMethods.SHGetFolderPath(
                            IntPtr.Zero,
                            NativeMethods.SpecialFolderCSIDL.CSIDL_STARTMENU,
                            IntPtr.Zero,
                            0
                        ),
                        _configuration.StartMenu,
                        Program.Configuration.Title + ".lnk"
                    ),
                    mainIcon
                );
            }

            if (_configuration.CreateDesktopShortcut)
            {
                AddLog(Labels.CreatingDesktopShortcut);

                CreateShortcut(
                    Path.Combine(
                        NativeMethods.SHGetFolderPath(
                            IntPtr.Zero,
                            NativeMethods.SpecialFolderCSIDL.CSIDL_DESKTOP,
                            IntPtr.Zero,
                            0
                        ),
                        Program.Configuration.Title + ".lnk"
                    ),
                    mainIcon
                );
            }
        }

        private string FindMainIcon()
        {
            using (var contextKey = PackageRegistry.OpenRegistryRoot(false, Program.Configuration.Context))
            {
                string corePackageId = null;
                var guid = new Guid();

                using (var installedProductsKey = contextKey.OpenSubKey("InstalledProducts"))
                {
                    foreach (string packageId in installedProductsKey.GetSubKeyNames())
                    {
                        // Only the core package can provide an icon.

                        if (PackageManager.IsCorePackage(packageId, Program.Configuration.Context))
                        {
                            corePackageId = packageId;

                            using (var packageKey = installedProductsKey.OpenSubKey(packageId))
                            {
                                guid = Guid.Parse((string)packageKey.GetValue("Package"));
                            }
                            break;
                        }
                    }
                }

                if (corePackageId == null)
                    return null;

                using (var iconKey = contextKey.OpenSubKey("Packages\\" + guid.ToString("B") + "\\ApplicationIcon"))
                {
                    if (iconKey != null)
                    {
                        return Path.Combine(
                            _configuration.InstallationPath,
                            "Packages",
                            corePackageId,
                            (string)iconKey.GetValue(null)
                        );
                    }
                }
            }

            return null;
        }

        private void CreateShortcut(string shortcutFileName, string mainIcon)
        {
            var shellLink = new ShellLink
            {
                Target = PackageManager.GetEntryAssemblyLocation(_configuration.InstallationPath),
                WorkingDirectory = _configuration.InstallationPath
            };

            if (mainIcon != null)
                shellLink.IconPath = mainIcon;

            Directory.CreateDirectory(Path.GetDirectoryName(shortcutFileName));

            shellLink.Save(shortcutFileName);
        }
    }
}
