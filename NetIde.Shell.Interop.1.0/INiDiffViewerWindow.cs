using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiDiffViewerWindow : INiWindowPane, INiConnectionPoint
    {
        HResult Advise(INiDiffViewerWindowNotify sink, out int cookie);
        HResult GetMode(out NiDiffViewerMode mode);
        HResult SetMode(NiDiffViewerMode mode);
        HResult Reset();
        HResult Load(IStream left, IStream right);
        HResult GetLeft(out IStream stream);
        HResult GetRight(out IStream stream);
        HResult SetReadOnly(bool readOnly);
        HResult GetReadOnly(out bool readOnly);
    }
}
