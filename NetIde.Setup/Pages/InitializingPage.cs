using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using NetIde.Setup.Installation;
using NetIde.Update;
using NetIde.Util;

namespace NetIde.Setup.Pages
{
    public partial class InitializingPage : PageControl
    {
        public InitializingPage()
        {
            InitializeComponent();
        }

        private void InitializingPage_Load(object sender, EventArgs e)
        {
            new Thread(ThreadProc).Start();
        }

        private void ThreadProc()
        {
            try
            {
                MainForm.SetupConfiguration.Packages.AddRange(
                    new PackageResolver().ResolvePackages(Program.Configuration)
                );

                BeginInvoke(new Action(() =>
                {
                    // See whether we're installing a plugin and the context
                    // is not yet available.

                    var primaryPackage = MainForm.SetupConfiguration.Packages.SingleOrDefault(p =>
                        PackageManager.IsCorePackage(p.Metadata.Id, Program.Configuration.Context)
                    );

                    // The primary package should always be there from the dependency
                    // resolver.

                    Debug.Assert(primaryPackage != null);

                    if (
                        primaryPackage == null ||
                        (!primaryPackage.IsInConfiguration && Program.Configuration.InstallationPath == null)
                    ) {
                        string contextTitle =
                            primaryPackage != null
                            ? primaryPackage.Metadata.Title
                            : Program.Configuration.Context.Name;

                        MainForm.ShowMessage(
                            MessageSeverity.Error,
                            String.Format(
                                Labels.ContextNotInstalled,
                                contextTitle
                            )
                        );
                        return;
                    }

                    // If all packages are up to date, quit the setup and tell
                    // the user there was nothing to do.

                    bool allUpToDate = true;

                    foreach (var package in MainForm.SetupConfiguration.Packages.Where(p => p.IsInConfiguration))
                    {
                        if (package.InstalledVersion != package.Metadata.Version)
                        {
                            allUpToDate = false;
                            break;
                        }
                    }

                    if (allUpToDate)
                    {
                        MainForm.ShowMessage(
                            MessageSeverity.Error,
                            Labels.AllPackagesUpToDate
                        );
                        return;
                    }

                    // Determine the mode. If any of the packages that are
                    // mentioned in the configuration are already present on
                    // the system, we're updating (even when we also have a new
                    // package). Otherwise, we're installing.

                    MainForm.SetupConfiguration.Mode = SetupMode.Install;

                    foreach (var package in MainForm.SetupConfiguration.Packages.Where(p => p.IsInConfiguration))
                    {
                        if (package.InstalledVersion != null)
                        {
                            MainForm.SetupConfiguration.Mode = SetupMode.Update;
                            break;
                        }
                    }

                    // If we have work to do, and we can actually do it,
                    // proceed to the next step.

                    MainForm.SetStep(1);
                }));
            }
            catch (Exception ex)
            {
                BeginInvoke(new Action(() =>
                    MainForm.ShowMessage(
                        MessageSeverity.Error,
                        String.Format(
                            Labels.UnexpectedSituation,
                            ex.Message
                        )
                    )
                ));
            }
        }
    }
}
