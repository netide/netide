using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiRunningDocumentIterator : INiIterator
    {
        HResult GetCurrent(out int cookie);
    }
}
