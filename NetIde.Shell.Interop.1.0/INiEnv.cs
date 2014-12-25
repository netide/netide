using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiEnv : INiConnectionPoint
    {
        INiWindow MainWindow { get; }
        INiWindowPane ActiveDocument { get; }
        string ContextName { get; }
        bool Experimental { get; }
        string FileSystemRoot { get; }
        string RegistryRoot { get; }
        string NuGetSite { get; set; }

        HResult Advise(INiEnvNotify sink, out int cookie);
        HResult Quit();
        HResult ExecuteCommand(Guid command, object argument);
        HResult RestartApplication();
        HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory);
        HResult SaveAllDocuments(NiSaveAllMode mode, bool prompt);
        HResult CloseAllDocuments(NiSaveAllMode mode);
    }
}
