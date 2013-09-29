using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Setup.Pages
{
    public enum Page
    {
        [Page(typeof(MessagePage))]
        Message,
        [Page(typeof(InitializingPage))]
        Initializing,
        [Page(typeof(WelcomePage))]
        Welcome,
        [Page(typeof(LicensePage))]
        License,
        [Page(typeof(InstallationPathPage))]
        InstallationPath,
        [Page(typeof(StartMenuPage))]
        StartMenu,
        [Page(typeof(ConfirmPage))]
        Confirm,
        [Page(typeof(InstallingPage))]
        Installing,
        [Page(typeof(FinishedPage))]
        Finished
    }
}
