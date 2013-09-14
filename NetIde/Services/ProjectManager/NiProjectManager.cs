using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.LocalRegistry;
using NetIde.Services.OpenDocumentManager;
using NetIde.Services.PackageManager;
using NetIde.Services.RunningDocumentTable;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Services.ProjectManager
{
    internal class NiProjectManager : ServiceBase, INiProjectManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(NiProjectManager));

        private readonly NiConnectionPoint<INiProjectManagerNotify> _connectionPoint = new NiConnectionPoint<INiProjectManagerNotify>();
        private INiProject _activeProject;
        private readonly Dictionary<Guid, INiProjectFactory> _factories = new Dictionary<Guid, INiProjectFactory>();

        public INiProject ActiveProject
        {
            get { return _activeProject; }
            private set
            {
                if (_activeProject != value)
                {
                    _activeProject = value;
                    _connectionPoint.ForAll(p => p.OnActiveProjectChanged());
                }
            }
        }

        public NiProjectManager(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiProjectManagerNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        public HResult OpenProject(string fileName)
        {
            return OpenProject(fileName, NiProjectCreateMode.Open);
        }

        public HResult CreateProject(string fileName)
        {
            return OpenProject(fileName, NiProjectCreateMode.Overwrite);
        }

        private HResult OpenProject(string fileName, NiProjectCreateMode mode)
        {
            try
            {
                if (fileName == null)
                    throw new ArgumentNullException("fileName");

                // Find the associated factory.

                INiProjectFactory factory = null;

                string extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    Debug.Assert(extension[0] == '.' && extension.Length > 1);

                    factory = FindProjectFactory(extension.Substring(1));
                }

                if (factory == null)
                    throw new ArgumentException(Labels.CannotFindProjectFactory, "fileName");

                // Close the existing project.

                if (ActiveProject != null)
                {
                    var activeProject = ActiveProject;
                    ActiveProject = null;
                    activeProject.Close();
                }

                // Create and load the new project.

                INiProject project;
                var result = factory.CreateProject(
                    fileName,
                    mode,
                    out project
                );

                if (ErrorUtil.Failure(result))
                    return result;

                project.SetPropertyEx(NiHierarchyProperty.OwnerType, factory.GetType().GUID);

                ActiveProject = project;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private INiProjectFactory FindProjectFactory(string extension)
        {
            var localRegistry = (NiLocalRegistry)GetService(typeof(INiLocalRegistry));

            foreach (var registration in localRegistry.Registrations.OfType<ProjectFactoryRegistration>())
            {
                foreach (string registrationExtension in registration.PossibleProjectExtensions.Split(';'))
                {
                    if (String.Equals(extension, registrationExtension.Trim(), StringComparison.OrdinalIgnoreCase))
                    {
                        INiProjectFactory factory;
                        _factories.TryGetValue(registration.Id, out factory);
                        return factory;
                    }
                }
            }

            return null;
        }

        public HResult OpenProjectViaDialog(string startDirectory)
        {
            try
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = Labels.OpenProject;

                    string fileName = CreateOpenProjectViaDialog(dialog, startDirectory);

                    if (fileName != null)
                    {
                        var hr = ((INiProjectManager)GetService(typeof(INiProjectManager))).CloseProject();

                        if (ErrorUtil.Failure(hr) || hr == HResult.False)
                            return hr;

                        hr = OpenProject(dialog.FileName);

                        if (ErrorUtil.Success(hr))
                            return HResult.OK;

                        var shell = (NiShell)GetService(typeof(INiShell));

                        ErrorUtil.ThrowOnFailure(shell.ShowMessageBox(
                            Labels.CannotOpenProject,
                            null,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        ));
                    }

                    return HResult.False;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CreateProjectViaDialog(string startDirectory)
        {
            try
            {
                using (var dialog = new SaveFileDialog())
                {
                    dialog.Title = Labels.CreateProject;

                    string fileName = CreateOpenProjectViaDialog(dialog, startDirectory);

                    if (fileName != null)
                    {
                        var hr = ((INiProjectManager)GetService(typeof(INiProjectManager))).CloseProject();

                        if (ErrorUtil.Failure(hr) || hr == HResult.False)
                            return hr;

                        hr = CreateProject(dialog.FileName);

                        if (ErrorUtil.Success(hr))
                            return HResult.OK;

                        var shell = (NiShell)GetService(typeof(INiShell));

                        ErrorUtil.ThrowOnFailure(shell.ShowMessageBox(
                            Labels.CannotCreateProject,
                            null,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        ));
                    }

                    return HResult.False;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private string CreateOpenProjectViaDialog(FileDialog dialog, string startDirectory)
        {
            // Build the filter from all registered project factories.

            var sb = new StringBuilder();
            var localRegistry = (NiLocalRegistry)GetService(typeof(INiLocalRegistry));
            var packageManager = (NiPackageManager)GetService(typeof(INiPackageManager));

            foreach (var registration in localRegistry.Registrations.OfType<ProjectFactoryRegistration>())
            {
                sb.Append(packageManager.Packages[registration.Package].Package.ResolveStringResource(registration.ProjectFileExtensions));
                sb.Append('|');
            }

            sb.Append(Labels.AllFiles);

            dialog.Filter = sb.ToString();

            if (startDirectory != null)
                dialog.InitialDirectory = startDirectory;
            else
                dialog.RestoreDirectory = true;

            var shell = (NiShell)GetService(typeof(INiShell));

            if (dialog.ShowDialog(shell.GetActiveWindow()) == DialogResult.OK)
                return dialog.FileName;

            return null;
        }

        public HResult RegisterProjectFactory(Guid guid, INiProjectFactory projectFactory)
        {
            try
            {
                if (projectFactory == null)
                    throw new ArgumentNullException("projectFactory");

                _factories.Add(guid, projectFactory);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult CloseProject()
        {
            try
            {
                if (ActiveProject == null)
                    return HResult.OK;

                var hr = ((INiEnv)GetService(typeof(INiEnv))).CloseAllDocuments(NiSaveAllMode.All);

                if (ErrorUtil.Failure(hr) || hr == HResult.False)
                    return hr;

                if (((NiOpenDocumentManager)GetService(typeof(INiOpenDocumentManager))).HaveOpenDocuments)
                    throw new InvalidOperationException(Labels.OpenDocumentsPresent);

                var activeProject = ActiveProject;

                ActiveProject = null;

                ErrorUtil.ThrowOnFailure(activeProject.Close());

                if (((NiRunningDocumentTable)GetService(typeof(INiRunningDocumentTable))).HaveOpenDocuments)
                    Log.Warn("There are still documents in the running document table after closing the project");

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult OpenProjectFromCommandLine()
        {
            try
            {
                var commandLine = (INiCommandLine)GetService(typeof(INiCommandLine));

                string[] arguments;
                ErrorUtil.ThrowOnFailure(commandLine.GetOtherArguments(out arguments));

                foreach (string argument in arguments)
                {
                    if (!File.Exists(argument))
                        continue;

                    string extension = Path.GetExtension(argument);

                    if (!String.IsNullOrEmpty(extension) && extension[0] == '.')
                    {
                        var factory = FindProjectFactory(extension.Substring(1));

                        if (factory != null)
                            return OpenProject(argument);
                    }
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
