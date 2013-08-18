using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using NetIde.Services.CommandLine;
using NetIde.Services.CommandManager;
using NetIde.Services.Env;
using NetIde.Services.JobManager;
using NetIde.Services.LocalRegistry;
using NetIde.Services.Logger;
using NetIde.Services.MenuManager;
using NetIde.Services.Shell;
using NetIde.Services.PackageManager;
using NetIde.Services.ToolsOptions;
using NetIde.Services.WindowPaneSelection;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;
using log4net;

namespace NetIde
{
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var serviceContainer = new NiServiceContainer())
            using (var mainForm = new MainForm())
            {
                serviceContainer.AddService(typeof(INiEnv), new NiEnv(serviceContainer, mainForm));

                // Show the splash form.

                string pathToSplashImage = FindPathToSplashImage(serviceContainer);

                if (pathToSplashImage != null)
                {
                    var splashForm = SplashForm.ShowSplashForm(pathToSplashImage);

                    mainForm.Shown += (s, e) =>
                    {
                        mainForm.Activate();
                        splashForm.Dispose();
                    };
                }

                // Continue initialization.

                serviceContainer.AddService(typeof(INiCommandLine), new NiCommandLine(serviceContainer, args));
                serviceContainer.AddService(typeof(INiLogger), new NiLogger(serviceContainer));

                LoggingRedirection.Install(serviceContainer);
                Log.Info("Starting NetIDE");

                serviceContainer.AddService(typeof(INiLocalRegistry), new NiLocalRegistry(serviceContainer));
                serviceContainer.AddService(typeof(INiCommandManager), new NiCommandManager(serviceContainer));
                serviceContainer.AddService(typeof(INiMenuManager), new NiMenuManager(serviceContainer));
                serviceContainer.AddService(typeof(INiWindowPaneSelection), new NiWindowPaneSelection(serviceContainer));
                serviceContainer.AddService(typeof(INiShell), new NiShell(serviceContainer));
                serviceContainer.AddService(typeof(INiJobManager), new NiJobManager(serviceContainer));
                serviceContainer.AddService(typeof(INiToolsOptions), new NiToolsOptions(serviceContainer));

                var packageManager = new NiPackageManager(serviceContainer);
                serviceContainer.AddService(typeof(INiPackageManager), packageManager);

                // Initialize the required services.

                mainForm.Site = new SiteProxy(serviceContainer);

                packageManager.Initialize();

                // Run the main form.

                Application.Run(mainForm);
            }
        }

        private static string FindPathToSplashImage(IServiceProvider serviceProvider)
        {
            // The logic in this method repeats (part of) the logic that the
            // package manager normally provides. The reason we repeat this here
            // is that we want to show the splash form as early as possible.
            // To achieve this, we need to read the available splash image from
            // the package configuration.

            var images = new Dictionary<Guid, string>();

            RegistryUtil.ForEachPackage(serviceProvider, "StartupSplashImage", (id, key) =>
            {
                string fileName = key.GetValue(null) as string;

                if (fileName != null)
                    images.Add(id, fileName);
            });

            if (images.Count == 0)
                return null;

            var env = (INiEnv)serviceProvider.GetService(typeof(INiEnv));
            string context = env.Context;

            // Only the contexts' core package is allowed to provide a splash
            // image. Otherwise plugins would be allowed to override the
            // splash image.

            string package = context + ".Package.Core";

            using (var baseKey = Registry.CurrentUser.OpenSubKey("Software\\Net IDE\\" + context + "\\InstalledProducts"))
            using (var key = baseKey.OpenSubKey(package))
            {
                Debug.Assert(key != null);

                if (key == null)
                {
                    Log.WarnFormat("Expected core package to be named '{0}'", package);
                    return null;
                }

                string productGuid = key.GetValue("Package") as string;
                Guid productId;
                string splashImage;

                if (
                    productGuid != null &&
                    Guid.TryParse(productGuid, out productId) &&
                    images.TryGetValue(productId, out splashImage)
                )
                {
                    string fileName = Path.Combine(
                        env.FileSystemRoot,
                        "Packages",
                        package,
                        splashImage
                    );

                    if (File.Exists(fileName))
                        return fileName;

                    Log.WarnFormat("Could not find registered splash image at '{0}'", fileName);
                }
                else
                {
                    Log.Warn("Registered splash image was not of core package");
                }
            }

            return null;
        }
    }
}
