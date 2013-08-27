using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextLines : INiTextBuffer, INiConnectionPoint
    {
        HResult Advise(INiTextLinesEvents sink, out int cookie);
        HResult GetLineText(int startLine, int startIndex, int endLine, int endIndex, out string result);
        HResult ReplaceLines(int startLine, int startIndex, int endLine, int endIndex, string text);
        HResult CreateTextMarker(NiTextMarkerType type, NiTextMarkerHatchStyle hatchStyle, bool extendToBorder, int color, int foreColor, int startLine, int startIndex, int endLine, int endIndex, out INiTextMarker textMarker);
    }
}
