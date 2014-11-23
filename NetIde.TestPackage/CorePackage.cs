using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.TestPackage.Api;

namespace NetIde.TestPackage
{
    [Guid(TPResources.PackageGuid)]
    [Description("@PackageDescription")]
    [NiResources("NiResources")]
    [NiStringResources("Labels")]
    [ProvideStartupSplashImage("Splash.png")]
    [ProvideProjectFactory(typeof(ProjectFactory), "@PackageDescription", "@ProjectFileExtensions", "niproj", "niproj")]
    public sealed partial class CorePackage : NiPackage, INiCommandTarget
    {
        private readonly NiCommandMapper _commandMapper = new NiCommandMapper();

        public INiEnv Env { get; private set; }

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();
                if (ErrorUtil.Failure(hr))
                    return hr;

                Env = (INiEnv)GetService(typeof(INiEnv));

                RegisterProjectFactory(new ProjectFactory());

                Env.MainWindow.Caption = Labels.PackageDescription;
                Env.MainWindow.Icon = Resources.MainIcon;

                BuildCommandMapper();

                ErrorUtil.ThrowOnFailure(
                    ((INiProjectExplorer)GetService(typeof(INiProjectExplorer))).Show()
                );

                RegisterHelp();

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

        private void RegisterHelp()
        {
            var help = (INiHelp)GetService(typeof(INiHelp));

#if DEBUG
            string path = ((INiEnv)GetService(typeof(INiEnv))).FileSystemRoot;

            while (path != null)
            {
                string source = Path.Combine(path, "NetIde.TestPackage.Help");
                if (Directory.Exists(source))
                {
                    ErrorUtil.ThrowOnFailure(help.Register("test", source));
                    break;
                }

                path = Path.GetDirectoryName(path);
            }
#else
            var packageManager = ((INiPackageManager)GetService(typeof(INiPackageManager)));

            string installationPath;
            ErrorUtil.ThrowOnFailure(packageManager.GetInstallationPath(this, out installationPath));

            string helpFileName = Path.Combine(installationPath, "Help.zip");

            ErrorUtil.ThrowOnFailure(help.Register("test", helpFileName));
#endif

            ErrorUtil.ThrowOnFailure(help.SetDefaultRoot("test"));
        }
    }
}
