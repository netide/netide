using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell
{
    internal static class AppDomainSetup
    {
        static AppDomainSetup()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public static void Setup()
        {
            // Empty because this is here just to trigger the static constructor.
        }
    }
}
