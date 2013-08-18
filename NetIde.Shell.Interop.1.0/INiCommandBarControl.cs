using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiCommandBarControl : IDisposable
    {
        Guid Id { get; }
        string Text { get; set; }
        string ToolTip { get; set; }
        bool IsVisible { get; set; }
        bool IsEnabled { get; set; }
        int Priority { get; }
    }
}
