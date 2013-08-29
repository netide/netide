using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiWindowFrameNotify
    {
        void OnShow(NiWindowShow action);
        void OnClose(NiFrameCloseMode closeMode, ref bool cancel);
        void OnSize();
    }
}
