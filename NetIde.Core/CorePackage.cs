using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.OptionPages.Environment;
using NetIde.Core.PackageManagement;
using NetIde.Core.Services.LanguageServiceRegistry;
using NetIde.Core.Services.ProjectExplorer;
using NetIde.Core.ToolWindows.DiffViewer;
using NetIde.Core.ToolWindows.ProjectExplorer;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Core.ToolWindows.TextEditor;

namespace NetIde.Core
{
    [Guid(Constants.CorePackageGuid)]
    [Description("@PackageDescription")]
    [ProvideEditorFactory(typeof(TextEditorFactory), "@TextEditorName")]
    [ProvideEditorFactory(typeof(DiffViewerFactory), "@DiffViewerName")]
    [ProvideCommandLineSwitch("Log", true)]
    [NiResources("NiResources")]
    [NiStringResources("Labels")]
    [ProvideOptionPage(typeof(FontsOptionPage), "Environment", "Fonts", "@Environment", "@Fonts")]
    [ProvideToolWindow(typeof(ProjectExplorerWindow), Style = NiDockStyle.Tabbed, Orientation = NiToolWindowOrientation.Right)]
    internal class CorePackage : NiPackage, INiCommandTarget
    {
        private readonly NiCommandMapper _commandMapper = new NiCommandMapper();

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (!ErrorUtil.Success(hr))
                    return hr;

                var serviceContainer = (IServiceContainer)GetService(typeof(IServiceContainer));

                serviceContainer.AddService(
                    typeof(INiLanguageServiceRegistry),
                    new NiLanguageServiceRegistry(this),
                    true
                );
                serviceContainer.AddService(
                    typeof(INiSettings),
                    new NiSettings(this),
                    true
                );
                serviceContainer.AddService(
                    typeof(INiProjectExplorer),
                    new NiProjectExplorer(this),
                    true
                );

                SetupCommands();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void SetupCommands()
        {
            _commandMapper.Add(
                Shell.NiResources.File_Exit,
                p => ErrorUtil.ThrowOnFailure(((INiEnv)GetService(typeof(INiEnv))).MainWindow.Close())
            );
            _commandMapper.Add(
                Shell.NiResources.Tools_Options,
                p => ErrorUtil.ThrowOnFailure(((INiToolsOptions)GetService(typeof(INiToolsOptions))).Open())
            );
            _commandMapper.Add(
                Shell.NiResources.Tools_PackageManagement,
                p => OpenPackageManagementForm()
            );
        }

        private void OpenPackageManagementForm()
        {
            using (var form = new PackageManagementForm())
            {
                form.ShowDialog(this);
            }
        }

        public HResult QueryStatus(Guid command, out NiCommandStatus status)
        {
            return _commandMapper.QueryStatus(command, out status);
        }

        public HResult Exec(Guid command, object argument, out object result)
        {
            return _commandMapper.Exec(command, argument, out result);
        }
    }
}
