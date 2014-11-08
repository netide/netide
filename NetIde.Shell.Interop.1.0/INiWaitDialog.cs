using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetIde.Shell.Interop
{
    public interface INiWaitDialog : IDisposable
    {
        HResult ShowWaitDialog(string caption, string message, string progressText, string statusBarText, TimeSpan showDelay, bool canCancel, bool realProgress, float progress, IntPtr[] waitHandles);
        HResult UpdateProgress(string message, string progressText, string statusBarText, float progress, bool disableCancel, out bool cancelled);
        HResult HasCanceled(out bool cancelled);
    }
}
