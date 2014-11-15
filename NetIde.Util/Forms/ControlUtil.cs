using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Util.Forms
{
    public class ControlUtil
    {
        public static bool GetIsInDesignMode(Control control)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return true;

            for (; control != null; control = control.Parent)
            {
                if (control.Site != null && control.Site.DesignMode)
                    return true;
            }

            return false;
        }

        public static Control GetRoot(Control self)
        {
            while (self.Parent != null)
            {
                self = self.Parent;
            }

            return self;
        }
    }
}
