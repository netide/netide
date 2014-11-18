using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiNotificationItem
    {
        HResult GetSubject(out string subject);
        HResult GetMessage(out string message);
        HResult GetPriority(out NiNotificationItemPriority priority);
        HResult GetCreated(out DateTime? created);
    }
}
