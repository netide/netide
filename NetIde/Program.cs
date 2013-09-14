using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetIde
{
    internal static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new NetIdeApplication(args).Run();
        }
    }
}
