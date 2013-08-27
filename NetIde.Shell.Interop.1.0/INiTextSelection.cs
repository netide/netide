using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextSelection
    {
        HResult GetCaretPosition(out int line, out int index);
        HResult SetCaretPosition(int line, int index);
        HResult GetSelectionCount(out int count);
        HResult GetSelection(int index, out int startLine, out int startIndex, out int endLine, out int endIndex);
        HResult SetSelection(int startLine, int startIndex, int endLine, int endIndex);
        HResult ClearSelection();
    }
}
