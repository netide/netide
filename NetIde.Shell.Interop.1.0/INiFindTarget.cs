using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiFindTarget : INiTextBufferProvider
    {
        HResult Find(string search, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result);
        HResult Replace(string search, string replace, NiFindOptions options, bool resetStartPoint, INiFindHelper helper, out NiFindResult result);
        HResult GetCurrentSpan(out NiTextSpan span);
        HResult GetFindState(out object state);
        HResult SetFindState(object state);
        HResult MarkSpan(NiTextSpan span);
        HResult NavigateTo(NiTextSpan span);
        HResult NotifyFindTarget(NiFindTargetNotify notify);
        HResult GetCapabilities(out NiFindCapabilities options);
        HResult GetInitialPattern(out string pattern);
    }
}
