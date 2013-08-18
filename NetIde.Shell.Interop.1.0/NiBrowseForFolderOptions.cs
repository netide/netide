using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Flags]
    public enum NiBrowseForFolderOptions
    {
        None = 0,
        HideEditBox = 1,
        HideNewFolderButton = 2
    }
}
