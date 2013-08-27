using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiTextMarker : IDisposable
    {
        HResult GetType(out NiTextMarkerType type);
        HResult GetColor(out int color);
        HResult GetForeColor(out int foreColor);
        HResult GetHatchStyle(out NiTextMarkerHatchStyle hatchStyle);
        HResult GetExtendToBorder(out bool extendToBorder);
        HResult GetIsReadOnly(out bool isReadOnly);
        HResult SetIsReadOnly(bool isReadOnly);
        HResult GetToolTip(out string toolTip);
        HResult SetToolTip(string toolTip);
        HResult GetOffset(out int startLine, out int startIndex, out int endLine, out int endIndex);
    }
}
