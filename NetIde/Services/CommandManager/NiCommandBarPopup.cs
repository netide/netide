using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal class NiCommandBarPopup : NiCommandBar, INiCommandBarPopup
    {
        public NiCommandBarPopup(Guid id, int priority)
            : base(id, NiCommandBarKind.Popup, priority)
        {
        }
    }
}
