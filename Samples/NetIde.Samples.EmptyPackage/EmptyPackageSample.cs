using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Samples.EmptyPackage
{
    [Guid("586bf7e4-a5aa-4cb9-95e0-00a7227809ff")]
    [Description("@PackageDescription")]
    [NiResources("NiResources")]
    [NiStringResources("Labels")]
    [ProvideStartupSplashImage("Resources\\Splash.png")]
    [ProvideApplicationIcon("Resources\\MainIcon.ico")]
    public sealed class EmptyPackageSample : NiPackage
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

                Env.MainWindow.Caption = Labels.PackageDescription;
                Env.MainWindow.Icon = Resources.MainIcon;

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
