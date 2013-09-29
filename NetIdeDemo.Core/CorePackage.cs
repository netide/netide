using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIdeDemo.Api;

namespace NetIdeDemo.Core
{
    [Guid(NidResources.PackageGuid)]
    [Description("@PackageDescription")]
    [NiResources("NiResources")]
    [NiStringResources("Labels")]
    [ProvideStartupSplashImage("Resources\\Splash.png")]
    [ProvideApplicationIcon("Resources\\MainIcon.ico")]
    public sealed class CorePackage : NiPackage, INiCommandTarget
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

                Env.MainWindow.Caption = Labels.PackageDescription;
                Env.MainWindow.Icon = Resources.MainIcon;

                BuildCommands();

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        private void BuildCommands()
        {
            var shell = (INiShell)GetService(typeof(INiShell));

            _commandMapper.Add(
                NidResources.About_About,
                p => ErrorUtil.ThrowOnFailure(shell.ShowMessageBox(
                    Labels.AboutMessage,
                    null,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                ))
            );
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
