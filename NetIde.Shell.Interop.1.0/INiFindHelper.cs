using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiFindHelper
    {
        HResult FindInText(string find, string replace, NiFindOptions options, string text, int offset, out int found, out int matchLength, out string replacementText, out bool isFound);
    }
}
