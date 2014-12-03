using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal interface IDiffMarker
    {
        DiffMarkerType Type { get; }
        int Line { get; }
        int Length { get; }
        int LeftLength { get; }
        int RightLength { get; }
    }
}
