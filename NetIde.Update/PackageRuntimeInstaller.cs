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

        public PackageRuntimeInstaller(string context, string packagePath)
            : base(context)
        {
            if (packagePath == null)
                throw new ArgumentNullException("packagePath");
            if (context == null)
                throw new ArgumentNullException("context");

            _packagePath = packagePath;
        }

        public override void Execute()
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
