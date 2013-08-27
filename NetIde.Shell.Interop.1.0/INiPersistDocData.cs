using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiPersistDocData
    {
        HResult IsDocDataDirty(out bool isDirty);
        HResult LoadDocData(string document);
        HResult SaveDocData(NiSaveMode mode, out string document, out bool saved);
    }
}
