using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiWindowFrame : INiConnectionPoint
    {
        bool IsVisible { get; }
        string Caption { get; set; }
        IResource Icon { get; set; }

        HResult Advise(INiWindowFrameNotify sink, out int cookie);
        HResult Show();
        HResult Hide();
        HResult Close();
    }
}
