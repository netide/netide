using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiShell
    {
        HResult CreateToolWindow(INiWindowPane windowPane, NiDockStyle dockStyle, NiToolWindowOrientation toolWindowOrientation, out INiWindowFrame toolWindow);
        HResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon);
        HResult ShowMessageBox(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, out DialogResult result);
        HResult BrowseForFolder(string title, NiBrowseForFolderOptions options, out string selectedFolder);
        HResult SaveDocDataToFile(NiSaveMode mode, INiPersistFile persistFile, string fileName, out string newFileName, out bool saved);
    }
}
