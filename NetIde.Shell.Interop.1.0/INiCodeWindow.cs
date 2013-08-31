using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCodeWindow :
        INiWindowPane,
        INiTextSelection,
        INiCommandTarget,
        INiStatusBarUser,
        INiFindTarget
    {
        HResult SetBuffer(INiTextLines textBuffer);
        HResult GetBuffer(out INiTextLines textBuffer);
    }
}
