using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiKeyboardMappings
    {
        HResult GetAllButtons(out INiCommandBarButton[] buttons);
        HResult GetKeys(INiCommandBarButton button, out Keys[] keys);
        HResult GetButtons(Keys keys, out INiCommandBarButton[] buttons);
        HResult SetKeys(INiCommandBarButton button, Keys[] keys);
    }
}
