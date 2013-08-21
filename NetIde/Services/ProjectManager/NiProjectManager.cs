using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetIde.Services.LocalRegistry;
using NetIde.Services.PackageManager;
using NetIde.Services.Shell;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Services.ProjectManager
{
    internal class NiProjectManager : ServiceBase, INiProjectManager
    {
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

        public HResult OpenProject(string fileName)
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
                    NiProjectCreateMode.Open,
                    out project
                );

                if (ErrorUtil.Failure(result))
                    return result;

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
                    dialog.Title = Labels.OpenProject;

                    var shell = (NiShell)GetService(typeof(INiShell));

                    if (dialog.ShowDialog(shell.GetActiveWindow()) == DialogResult.OK)
                    {
                        var result = OpenProject(dialog.FileName);

                        if (ErrorUtil.Failure(result))
                        {
                            ErrorUtil.ThrowOnFailure(shell.ShowMessageBox(
                                Labels.CannotOpenProject,
                                null,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error
                            ));

                            return HResult.False;
                        }

                        return HResult.OK;
                    }
                }

                return HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
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
    }
}
