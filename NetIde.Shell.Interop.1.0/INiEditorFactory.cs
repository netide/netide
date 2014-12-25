using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiEditorFactory : IServiceProvider, INiObjectWithSite
    {
        HResult CreateEditor(string document, out string editorCaption, out INiWindowPane editor);
    }
}
