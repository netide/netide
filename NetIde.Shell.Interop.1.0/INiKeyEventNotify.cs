using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiKeyEventNotify
    {
        void OnKeyDown(Keys key);
        void OnKeyUp(Keys key);
        void OnKeyPress(char key);
    }
}
