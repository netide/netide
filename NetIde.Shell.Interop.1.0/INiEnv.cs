using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiEnv
    {
        INiWindow MainWindow { get; }
        INiWindowPane ActiveDocument { get; }
        string Context { get; }
        string FileSystemRoot { get; }
        string RegistryRoot { get; }
        string NuGetSite { get; set; }

        HResult RestartApplication();
    }
}
