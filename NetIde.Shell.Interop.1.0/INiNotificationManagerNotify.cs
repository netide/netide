using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiNotificationManagerNotify
    {
        void OnClicked(int cookie);
        void OnDismissed(int cookie);
    }
}
