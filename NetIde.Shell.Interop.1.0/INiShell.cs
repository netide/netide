using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiShell : INiConnectionPoint
    {
        HResult Advise(INiShellEvents sink, out int cookie);
        HResult CreateToolWindow(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation toolWindowOrientation, out INiWindowFrame toolWindow);
        HResult BrowseForFolder(string title, NiBrowseForFolderOptions options, out string selectedFolder);
        HResult SaveDocDataToFile(NiSaveMode mode, INiPersistFile persistFile, string fileName, out string newFileName, out bool saved);
        HResult GetDocumentWindowIterator(out INiIterator<INiWindowFrame> iterator);
        HResult GetWindowFrameForWindowPane(INiWindowPane windowPane, out INiWindowFrame windowFrame);
        HResult BroadcastPreMessageFilter(ref NiMessage message);
        HResult InvalidateRequerySuggested();
        HResult CreateTaskDialog(out INiTaskDialog taskDialog);
    }
}
