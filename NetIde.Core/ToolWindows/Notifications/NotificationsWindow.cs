using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using NetIde.Core.Services.NotificationManager;
using NetIde.Shell;
using NetIde.Shell.Interop;
using NetIde.Util.Forms;

namespace NetIde.Core.ToolWindows.Notifications
{
    [Guid("59d3d206-9a79-4b6d-9b3f-2b91b3d3930d")]
    internal class NotificationsWindow : NiWindowPane
    {
        private NotificationsControl _control;

        public override HResult Initialize()
        {
            try
            {
                var hr = base.Initialize();

                if (ErrorUtil.Failure(hr))
                    return hr;

                ErrorUtil.ThrowOnFailure(Frame.SetCaption(Labels.Notifications));

                _control = new NotificationsControl
                {
                    Site = new SiteProxy(this),
                    Dock = DockStyle.Fill
                };

                Controls.Add(_control);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public void RedrawItems(List<NotificationItem> items)
        {
            _control.RedrawItems(items);
        }
    }
}
