using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiTitleBarButton : ServiceObject, INiTitleBarButton
    {
        public IResource Image { get; set; }

        public int Priority { get; set; }

        public Color? ForeColor { get; set; }

        public Color? BackColor { get; set; }

        public bool Enabled { get; set; }

        public bool Visible { get; set; }

        public NiTitleBarButton()
        {
            Enabled = true;
            Visible = true;
        }

        HResult INiTitleBarButton.GetImage(out IResource image)
        {
            image = Image;
            return HResult.OK;
        }

        HResult INiTitleBarButton.GetPriority(out int priority)
        {
            priority = Priority;
            return HResult.OK;
        }

        HResult INiTitleBarButton.GetForeColor(out Color? foreColor)
        {
            foreColor = ForeColor;
            return HResult.OK;
        }

        HResult INiTitleBarButton.GetBackColor(out Color? backColor)
        {
            backColor = BackColor;
            return HResult.OK;
        }

        HResult INiTitleBarButton.GetEnabled(out bool enabled)
        {
            enabled = Enabled;
            return HResult.OK;
        }

        HResult INiTitleBarButton.GetVisible(out bool visible)
        {
            visible = Visible;
            return HResult.OK;
        }
    }
}
