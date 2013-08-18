using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextLines : INiTextBuffer
    {
        HResult GetLineText(int startLine, int startIndex, int endLine, int endIndex, out string result);
        HResult ReplaceLines(int startLine, int startIndex, int endLine, int endIndex, string text);
    }
}
