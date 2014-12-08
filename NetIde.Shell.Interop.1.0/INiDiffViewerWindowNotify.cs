using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiDiffViewerWindowNotify
    {
        void OnModeChanged();
        void OnLeftChanging(out bool cancel);
        void OnLeftChanged();
        void OnRightChanging(out bool cancel);
        void OnRightChanged();
    }
}
