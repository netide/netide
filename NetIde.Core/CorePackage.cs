using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.OptionPages.Environment;
using NetIde.Core.Services.Finder;
using NetIde.Core.Services.LanguageServiceRegistry;
using NetIde.Core.Services.ProjectExplorer;
using NetIde.Core.ToolWindows.DiffViewer;
using NetIde.Core.ToolWindows.FindResults;
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
    [ProvideCommandLineSwitch("Experimental", false)]
    [NiResources("NiResources")]
    [NiStringResources("Labels")]
    [ProvideOptionPage(typeof(FontsOptionPage), "Environment", "Fonts", "@Environment", "@Fonts")]
    [ProvideToolWindow(typeof(ProjectExplorerWindow), Style = NiDockStyle.Tabbed, Orientation = NiToolWindowOrientation.Right)]
    [ProvideToolWindow(typeof(FindResultsWindow), Style = NiDockStyle.Tabbed, Orientation = NiToolWindowOrientation.Bottom)]
    [ProvideAllEditorExtensionsAttribute]
    internal partial class CorePackage : NiPackage, INiCommandTarget
    {
        private readonly NiCommandMapper _commandMapper = new NiCommandMapper();
        private INiProjectManager _projectManager;
        private INiWindowPaneSelection _windowPaneSelection;
        private INiEnv _env;
        private INiFinder _finder;

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

                serviceContainer.AddService(
                    typeof(INiFinder),
                    new NiFinder(this)
                );

                _projectManager = (INiProjectManager)GetService(typeof(INiProjectManager));
                _windowPaneSelection = (INiWindowPaneSelection)GetService(typeof(INiWindowPaneSelection));
                _env = (INiEnv)GetService(typeof(INiEnv));
                _finder = (INiFinder)GetService(typeof(INiFinder));

                BuildCommands();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
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
