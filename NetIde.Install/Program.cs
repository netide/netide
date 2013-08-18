using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetIde.Update;

namespace NetIde.Install
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Net IDE Package Registration");

            string package = null;
            string context = null;
            bool uninstall = false;

            for (int i = 0; i < args.Length; i++)
            {
                if (
                    String.Equals(args[i], "/context", StringComparison.OrdinalIgnoreCase) ||
                    String.Equals(args[i], "/c", StringComparison.OrdinalIgnoreCase)
                ) {
                    i++;

                    if (i < args.Length)
                        context = args[i];
                }
                else if (
                    String.Equals(args[i], "/uninstall", StringComparison.OrdinalIgnoreCase) ||
                    String.Equals(args[i], "/u", StringComparison.OrdinalIgnoreCase)
                ) {
                    uninstall = true;
                }
                else
                {
                    package = args[i];
                }
            }

            if (package == null || context == null)
            {
                Console.Error.WriteLine("Usage: niinstall.exe /context|c <context> [/uninstall|u] <package>");
                return 1;
            }

            if (!uninstall && !File.Exists(package))
            {
                Console.Error.WriteLine("Package not found");
                return 3;
            }

            try
            {
                if (uninstall)
                    new PackageUninstaller(context, package).Execute();
                else
                    new PackageInstaller(context, package).Execute();

                return 0;
            }
            catch (PackageInstallationException ex)
            {
                Console.WriteLine(ex.Message);

                return ex.ExitCode + 10;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Package could not be installed: " + ex.Message);

                if (ex.StackTrace != null)
                    Console.Error.WriteLine(ex.StackTrace);

                return 2;
            }
        }
    }
}
