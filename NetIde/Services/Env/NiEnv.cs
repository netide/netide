using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NetIde.Services.CommandLine;
using NetIde.Services.PackageManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Update;
using NetIde.Util;
using NetIde.Xml;
using NetIde.Xml.Context;
using log4net;

namespace NetIde.Services.Env
{
    internal class NiEnv : ServiceBase, INiEnv
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiEnv));

        private readonly string _initialWorkingDirectory;
        private bool _disposed;
        private readonly NiConnectionPoint<INiEnvNotify> _connectionPoint = new NiConnectionPoint<INiEnvNotify>();

        public MainForm MainForm { get; private set; }
        public ResourceManager ResourceManager { get; private set; }
        public INiWindow MainWindow { get; private set; }
        public string ContextName { get; private set; }
        public bool Experimental { get; private set; }
        public ContextName Context { get; private set; }
        public string NuGetSite { get; set; }
        public string FileSystemRoot { get; private set; }
        public string RegistryRoot { get; private set; }

        public INiWindowPane ActiveDocument
        {
            get { return MainForm.ActiveDocument; }
        }

        public NiEnv(IServiceProvider serviceProvider, MainForm mainForm, bool experimental)
            : base(serviceProvider)
        {
            if (mainForm == null)
                throw new ArgumentNullException("mainForm");

            Experimental = experimental;
            ResourceManager = new ResourceManager();
            MainForm = mainForm;
            MainWindow = mainForm.GetProxy();

            _initialWorkingDirectory = Environment.CurrentDirectory;

            LoadContext();
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiEnvNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        private void LoadContext()
        {
            LoadContext(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            if (ContextName == null)
                LoadContext(Environment.CurrentDirectory);

            if (ContextName != null)
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

                    if (ContextName != null)
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

            if (Experimental)
                registryRoot += "$Exp";

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

            ContextName = context.Name;
            Context = new ContextName(ContextName, Experimental);
            NuGetSite = context.NuGetSite;
            FileSystemRoot = fileSystemRoot;
            RegistryRoot = registryRoot;
        }

        public void CompleteStartup()
        {
            _connectionPoint.ForAll(p => p.OnStartupComplete());
        }

        public HResult Quit()
        {
            try
            {
                bool canClose = ((NiPackageManager)GetService(typeof(INiPackageManager))).QueryClose();

                if (!canClose)
                    return HResult.False;

                var hr = CloseAllDocuments(NiSaveAllMode.All);

                if (ErrorUtil.Failure(hr) || hr == HResult.False)
                    return hr;

                hr = ((INiProjectManager)GetService(typeof(INiProjectManager))).CloseProject();

                if (ErrorUtil.Failure(hr) || hr == HResult.False)
                    return hr;

                _connectionPoint.ForAll(p => p.OnBeginShutdown());

                MainForm.AllowQuit = true;

                ErrorUtil.ThrowOnFailure(MainWindow.Close());

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult RestartApplication()
        {
            try
            {
                var hr = Quit();

                if (ErrorUtil.Failure(hr) || hr == HResult.False)
                    return hr;

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

        public HResult ExecuteCommand(Guid command, object argument)
        {
            try
            {
                object result;
                ErrorUtil.ThrowOnFailure(((INiCommandManager)GetService(typeof(INiCommandManager))).Exec(
                    command, argument, out result
                ));

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CloseAllDocuments(NiSaveAllMode mode)
        {
            try
            {
                var hr = SaveAllDocuments(mode, true);

                if (ErrorUtil.Failure(hr) || hr == HResult.False)
                    return hr;

                foreach (var windowFrame in ((INiShell)GetService(typeof(INiShell))).GetDocumentWindows())
                {
                    // We specifically request the frame not to save, because
                    // the SaveAllDocuments should already have taken care of this
                    // and we may have dirty windows because the user said the
                    // documents shouldn't be saved.

                    ErrorUtil.ThrowOnFailure(windowFrame.Close(NiFrameCloseMode.NoSave));
                }

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SaveAllDocuments(NiSaveAllMode mode, bool prompt)
        {
            try
            {
                var dirtyDocuments = GetDirtyDocuments(mode);

                if (dirtyDocuments.Count == 0)
                    return HResult.OK;

                NiQuerySaveResult querySaveResult;
                ErrorUtil.ThrowOnFailure(((INiShell)GetService(typeof(INiShell))).QuerySaveViaDialog(
                    dirtyDocuments.Select(p => p.Item1).ToArray(),
                    out querySaveResult
                ));

                switch (querySaveResult)
                {
                    case NiQuerySaveResult.Save:
                        foreach (var docData in dirtyDocuments.Select(p => p.Item2))
                        {
                            string document;
                            bool saved;
                            ErrorUtil.ThrowOnFailure(docData.SaveDocData(
                                NiSaveMode.SilentSave,
                                out document,
                                out saved
                            ));

                            if (!saved)
                                Log.Warn("Document {0} was not saved during SaveAll; ignoring");
                        }
                        return HResult.OK;

                    case NiQuerySaveResult.DoNotSave:
                        return HResult.OK;

                    default:
                        return HResult.False;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private List<Tuple<INiHierarchy, INiPersistDocData>> GetDirtyDocuments(NiSaveAllMode mode)
        {
            var dirtyDocuments = new List<Tuple<INiHierarchy, INiPersistDocData>>();

            if (mode == NiSaveAllMode.VisibleOnly)
            {
                foreach (var windowFrame in ((INiShell)GetService(typeof(INiShell))).GetDocumentWindows())
                {
                    var docData = (INiPersistDocData)windowFrame.GetPropertyEx(NiFrameProperty.DocData);

                    if (docData != null)
                    {
                        bool isDirty;
                        ErrorUtil.ThrowOnFailure(docData.IsDocDataDirty(out isDirty));

                        if (isDirty)
                        {
                            var hier = (INiHierarchy)windowFrame.GetPropertyEx(NiFrameProperty.Hierarchy);

                            Debug.Assert(hier != null);

                            if (hier != null)
                                dirtyDocuments.Add(Tuple.Create(hier, docData));
                        }
                    }
                }
            }
            else
            {
                var runningDocumentTable = (INiRunningDocumentTable)GetService(typeof(INiRunningDocumentTable));

                foreach (int cookie in runningDocumentTable.GetDocuments())
                {
                    string document;
                    INiHierarchy hier;
                    INiPersistDocData docData;
                    ErrorUtil.ThrowOnFailure(runningDocumentTable.GetDocumentInfo(
                        cookie, out document, out hier, out docData
                    ));

                    bool isDirty;
                    ErrorUtil.ThrowOnFailure(docData.IsDocDataDirty(out isDirty));

                    if (isDirty)
                        dirtyDocuments.Add(Tuple.Create(hier, docData));
                }
            }

            return dirtyDocuments;
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
