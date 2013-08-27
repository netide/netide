using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiStatusBarProgress : IDisposable
    {
        HResult Update(string label, int progress, int total);
    }
}
