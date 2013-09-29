using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetIde.Update
{
    public class PackageRuntimeInstaller : PackageManager
    {
        private readonly string _packagePath;
        private readonly bool _inPlace;

        public PackageRuntimeInstaller(ContextName context, string packagePath)
            : this(context, packagePath, false)
        {
        }

        public PackageRuntimeInstaller(ContextName context, string packagePath, bool inPlace)
            : base(context)
        {
            if (packagePath == null)
                throw new ArgumentNullException("packagePath");
            if (context == null)
                throw new ArgumentNullException("context");

            _packagePath = packagePath;
            _inPlace = inPlace;
        }

        public override void Execute()
        {
            if (_inPlace)
                ExecuteInPlace();
            else
                ExecuteWithRestart();
        }

        private void ExecuteInPlace()
        {
            // The setup does an in place installation of the runtime package.

            string target = Path.Combine(GetFileSystemRoot(), "bin");

            // Delete the current target directory.

            if (Directory.Exists(target))
            {
                Directory.Delete(target, true);
                Directory.CreateDirectory(target);
            }

            // Perform the installation

            ExtractPackage(_packagePath, target);
        }

        private void ExecuteWithRestart()
        {
            // Extract the contents of the tools directory into a temporary
            // location which we use later to update the main application.

            string target = Path.Combine(GetFileSystemRoot(), "PendingUpdate");

            ExtractPackage(_packagePath, target);

            // Re-start ourselves from the temporary path and signal that
            // we'd like a runtime update.

            string fileName = Path.Combine(
                target,
                Path.GetFileName(Assembly.GetEntryAssembly().Location)
            );

            if (!File.Exists(fileName))
                throw new PackageInstallationException(Labels.CorruptRuntimePackage, 7);

            Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                WorkingDirectory = target,
                Arguments = "/RuntimeUpdate",
                UseShellExecute = false
            });
        }
    }
}
