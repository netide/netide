using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util;
using NetIde.Xml;
using NetIde.Xml.Context;

namespace NetIde.Services.Env
{
    internal class NiEnv : ServiceBase, INiEnv
    {
        private readonly string _initialWorkingDirectory;
        private bool _disposed;

        public MainForm MainForm { get; private set; }
        public ResourceManager ResourceManager { get; private set; }
        public INiWindow MainWindow { get; private set; }
        public string Context { get; private set; }
        public string NuGetSite { get; set; }
        public string FileSystemRoot { get; private set; }
        public string RegistryRoot { get; private set; }

        public INiWindowPane ActiveDocument
        {
            get { return MainForm.ActiveDocument; }
        }

        public NiEnv(IServiceProvider serviceProvider, MainForm mainForm)
            : base(serviceProvider)
        {
            if (mainForm == null)
                throw new ArgumentNullException("mainForm");

            ResourceManager = new ResourceManager();
            MainForm = mainForm;
            MainWindow = mainForm.GetProxy();

            _initialWorkingDirectory = Environment.CurrentDirectory;

            LoadContext();
        }

        private void LoadContext()
        {
            LoadContext(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));

            if (Context == null)
                LoadContext(Environment.CurrentDirectory);

            if (Context != null)
                return;

            MessageBox.Show(
                Labels.CannotLoadContext,
                Labels.NetIde,
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error
            );

            Environment.Exit(0);
        }

        private void LoadContext(string path)
        {
            while (path != null)
            {
                string fileName = Path.Combine(path, Xml.Context.Context.FileName);

                if (File.Exists(fileName))
                {
                    SEH.SinkExceptions(() => LoadContextFromFile(fileName));

                    if (Context != null)
                        break;
                }

                path = Path.GetDirectoryName(path);
            }
        }

        private void LoadContextFromFile(string fileName)
        {
            var context = Serialization.Deserialize<Context>(fileName);

            if (context.Name.IndexOfAny(new[] { '/', '\\' }) != -1)
                return;

            string registryRoot = "Software\\Net IDE\\" + context.Name;

            // Attempt to create the registry root to see whether the context
            // name is a legal name.

            string fileSystemRoot;

            using (var key = Registry.CurrentUser.OpenSubKey(registryRoot))
            {
                if (key == null)
                    return;

                fileSystemRoot = (string)key.GetValue("InstallationPath");
            }

            if (fileSystemRoot == null || !Directory.Exists(fileSystemRoot))
                return;

            Context = context.Name;
            NuGetSite = context.NuGetSite;
            FileSystemRoot = fileSystemRoot;
            RegistryRoot = registryRoot;
        }

        public HResult RestartApplication()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();

                Process.Start(new ProcessStartInfo
                {
                    WorkingDirectory = _initialWorkingDirectory,
                    UseShellExecute = false,
                    FileName = args[0],
                    Arguments = String.Join(" ", Environment.GetCommandLineArgs().Skip(1).Select(Escaping.ShellArgument))
                });

                Environment.Exit(0);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                if (ResourceManager != null)
                {
                    ResourceManager.Dispose();
                    ResourceManager = null;
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
