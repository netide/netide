using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiPersistFile
    {
        HResult GetFileName(out string fileName);
        HResult IsDirty(out bool isDirty);
        HResult Load(string fileName);
        HResult Save(string fileName, bool remember);
    }
}
