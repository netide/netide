using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiEditorFactory : IServiceProvider
    {
        HResult SetSite(IServiceProvider serviceProvider);
        HResult CreateEditor(out INiWindowPane editor);
    }
}
