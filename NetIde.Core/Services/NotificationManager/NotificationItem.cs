using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core.Services.NotificationManager
{
    internal class NotificationItem
    {
        public int Cookie { get; private set; }

        public string Subject { get; private set; }

        public string Message { get; private set; }

        public NiNotificationItemPriority Priority { get; private set; }

        public DateTime? Created { get; private set; }

        public NotificationItem(int cookie)
        {
            Cookie = cookie;
        }

        public void Update(INiNotificationItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            string subject;
            ErrorUtil.ThrowOnFailure(item.GetSubject(out subject));
            Subject = subject;

            string message;
            ErrorUtil.ThrowOnFailure(item.GetMessage(out message));
            Message = message;

            NiNotificationItemPriority priority;
            ErrorUtil.ThrowOnFailure(item.GetPriority(out priority));
            Priority = priority;

            DateTime? created;
            ErrorUtil.ThrowOnFailure(item.GetCreated(out created));
            Created = created;
        }
    }
}
