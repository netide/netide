using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiMenuManager
    {
        HResult RegisterCommandBar(INiCommandBar commandBar);
        HResult UnregisterCommandBar(INiCommandBar commandBar);
        HResult ShowContextMenu(Guid popupId, Point location);
    }
}
