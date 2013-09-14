using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed class CorePackage : NiPackage
    {
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

                ErrorUtil.ThrowOnFailure(
                    ((INiProjectExplorer)GetService(typeof(INiProjectExplorer))).Show()
                );

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
