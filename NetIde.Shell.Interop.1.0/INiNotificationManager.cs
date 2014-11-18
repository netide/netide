using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiNotificationManager : INiConnectionPoint
    {
        HResult Advise(INiNotificationManagerNotify sink, out int cookie);
        HResult AddItem(INiNotificationItem item, out int cookie);
        HResult UpdateItem(int cookie, INiNotificationItem item);
        HResult RemoveItem(int cookie);

        HResult Show();

        HResult Hide();
    }
}
