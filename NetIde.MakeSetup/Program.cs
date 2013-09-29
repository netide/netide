using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NetIde.MakeSetup
{
    internal static class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Net IDE Setup Baker");

            string nuGetWebsite = "https://www.nuget.org/api/v2/";
            string title = null;
            string context = null;
            string targetFileName = null;
            string license = null;
            string startMenu = null;
            var packages = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLowerInvariant())
                {
                    case "/t":
                    case "/title":
                        if (++i < args.Length)
                            title = args[i];
                        break;

                    case "/c":
                    case "/context":
                        if (++i < args.Length)
                            context = args[i];
                        break;

                    case "/nuget":
                    case "/n":
                        if (++i < args.Length)
                            nuGetWebsite = args[i];
                        break;

                    case "/out":
                    case "/o":
                        if (++i < args.Length)
                            targetFileName = args[i];
                        break;

                    case "/package":
                    case "/p":
                        if (++i < args.Length)
                            packages.Add(args[i]);
                        break;

                    case "/l":
                    case "/license":
                        if (++i < args.Length)
                            license = args[i];
                        break;

                    case "/s":
                    case "/startmenu":
                        if (++i < args.Length)
                            startMenu = args[i];
                        break;
                }
            }

            if (packages.Count == 0 || targetFileName == null)
            {
                Console.Error.WriteLine("Usage: nimakesetup.exe [/n|/nuget <nuget website>] /p|/package <package> ... /o|/out <target> /c|/context <context> /t|/title <setup title> [/l|/license <license text file>] [/s|/startmenu <start menu folder name>]");
                return 1;
            }

            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            Directory.CreateDirectory(tempPath);

            try
            {
                // Write the config file.

                using (var writer = File.CreateText(Path.Combine(tempPath, "config.ini")))
                {
                    writer.WriteLine("Context={0}", context);
                    writer.WriteLine("Title={0}", title);
                    writer.WriteLine("NuGetWebsite={0}", nuGetWebsite);
                    writer.WriteLine("Packages={0}", String.Join(";", packages));

                    if (startMenu != null)
                        writer.WriteLine("StartMenu={0}", startMenu);
                }

                // Create the base archive to which we add our setup.

                File.WriteAllBytes(Path.Combine(tempPath, "setup.7z"), NeutralResources.setup);

                // Create 7za.exe.

                string archiver = Path.Combine(tempPath, "7za.exe");
                File.WriteAllBytes(archiver, NeutralResources._7za);

                // Create the 7z archive.

                var files = new List<string>
                {
                    "config.ini"
                };

                if (license != null)
                {
                    File.Copy(license, Path.Combine(tempPath, "license.txt"));
                    files.Add("license.txt");
                }

                using (var process = Process.Start(new ProcessStartInfo
                {
                    Arguments = "a setup.7z " + String.Join(" ", files),
                    CreateNoWindow = true,
                    FileName = archiver,
                    UseShellExecute = false,
                    WorkingDirectory = tempPath
                }))
                {
                    process.WaitForExit();
                }

                // Create the target.

                using (var target = File.Create(targetFileName))
                {
                    using (var source = new MemoryStream(NeutralResources.NetIDE))
                    {
                        source.CopyTo(target);
                    }

                    using (var source = File.OpenRead(Path.Combine(tempPath, "setup.7z")))
                    {
                        source.CopyTo(target);
                    }
                }
            }
            finally
            {
                Directory.Delete(tempPath, true);
            }

            return 0;
        }
    }
}
