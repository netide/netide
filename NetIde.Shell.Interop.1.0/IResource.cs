using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface IResource
    {
        string Key { get; }
        string FileName { get; }

        HResult Open(out IStream stream);
    }
}
