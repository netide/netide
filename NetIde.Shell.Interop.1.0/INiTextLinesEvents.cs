using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextLinesEvents
    {
        void OnChanged(int startLine, int startIndex, int endLine, int endIndex);
    }
}
