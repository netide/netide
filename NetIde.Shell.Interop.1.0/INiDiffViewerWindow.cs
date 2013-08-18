using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiDiffViewerWindow : INiWindowPane, INiConnectionPoint
    {
        HResult Advise(INiDiffViewerWindowNotify sink, out int cookie);
        HResult GetState(out NiDiffViewerState state);
        HResult SetState(NiDiffViewerState state);
        HResult Reset();
        HResult Load(IStream left, IStream right);
    }
}
