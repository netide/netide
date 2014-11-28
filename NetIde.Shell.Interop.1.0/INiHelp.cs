using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiHelp
    {
        HResult Show();
        HResult Hide();
        HResult Home();
        HResult Navigate(string root, string path);
        HResult Register(string root, string source);
    }
}
