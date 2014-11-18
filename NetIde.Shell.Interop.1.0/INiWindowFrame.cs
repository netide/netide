using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiWindowFrame : INiConnectionPoint
    {
        HResult Advise(INiWindowFrameNotify sink, out int cookie);
        HResult Show();
        HResult Hide();
        HResult Close(NiFrameCloseMode closeMode);
        HResult GetProperty(int property, out object value);
        HResult SetProperty(int property, object value);
        HResult GetIsVisible(out bool visible);
        HResult GetCaption(out string caption);
        HResult SetCaption(string caption);
        HResult GetIcon(out IResource icon);
        HResult SetIcon(IResource icon);
    }
}
