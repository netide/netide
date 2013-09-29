using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetIde.Setup
{
    internal static class Program
    {
        public static Configuration Configuration { get; private set; }

        [STAThread]
        public static int Main()
        {
            try
            {
                Configuration = new Configuration();
            }
            catch
            {
                MessageBox.Show(
                    Labels.InvalidConfiguration,
                    Labels.NetIdePackageSetup,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return 1;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            return 0;
        }
    }
}
