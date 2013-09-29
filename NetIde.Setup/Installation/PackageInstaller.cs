using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Xml.PackageMetadata;
using File = System.IO.File;

namespace NetIde.Setup.Installation
{
    public partial class PackageInstaller : IDisposable
    {
        private readonly SetupConfiguration _configuration;
        private readonly IProgress _progress;
        private bool _disposed;
        private PackageMetadata[] _packages;

        public PackageInstaller(SetupConfiguration configuration, IProgress progress)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            if (progress == null)
                throw new ArgumentNullException("progress");

            _configuration = configuration;
            _progress = progress;
        }

        public void Install()
        {
            _packages = _configuration.Packages
                .Where(p => p.InstalledVersion != p.Metadata.Version)
                .Select(p => p.Metadata)
                .ToArray();

            CreateContext();
            DownloadPackages();
            InstallPackages();
            CreateShortcuts();
        }

        private void AddLog(string message, params object[] args)
        {
            if (args != null && args.Length > 0)
                message = String.Format(message, args);

            _progress.AddLog(message);
        }

        private void SetProgress(double progress)
        {
            _progress.SetProgress(progress);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_downloads != null)
                {
                    foreach (string tempFileName in _downloads.Values)
                    {
                        try
                        {
                            File.Delete(tempFileName);
                        }
                        catch
                        {
                            // Suppress exceptions.
                        }
                    }
                }

                _disposed = true;
            }
        }
    }
}
