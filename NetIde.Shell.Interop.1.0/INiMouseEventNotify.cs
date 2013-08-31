using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NetIde.Shell.Interop
{
    public interface INiMouseEventNotify
    {
        void OnMouseDown(MouseButtons button, int x, int y);
        void OnMouseUp(MouseButtons button, int x, int y);
        void OnMouseClick(MouseButtons button, int x, int y);
        void OnMouseDoubleClick(MouseButtons button, int x, int y);
        void OnMouseWheel(int delta);
    }
}
