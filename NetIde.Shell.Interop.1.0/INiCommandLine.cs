using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandLine
    {
        HResult GetOption(string name, out bool present, out string value);
        HResult GetOtherArguments(out string[] arguments);
    }
}
