using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiNotificationItem : ServiceObject, INiNotificationItem
    {
        public string Subject { get; set; }

        public string Message { get; set; }

        public NiNotificationItemPriority Priority { get; set; }

        public DateTime? Created { get; set; }

        HResult INiNotificationItem.GetSubject(out string subject)
        {
            subject = Subject;
            return HResult.OK;
        }

        HResult INiNotificationItem.GetMessage(out string message)
        {
            message = Message;
            return HResult.OK;
        }

        HResult INiNotificationItem.GetPriority(out NiNotificationItemPriority priority)
        {
            priority = Priority;
            return HResult.OK;
        }

        HResult INiNotificationItem.GetCreated(out DateTime? created)
        {
            created = Created;
            return HResult.OK;
        }
    }
}
