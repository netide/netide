using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiLocalRegistry
    {
        HResult RegisterEditorFactory(Guid guid, INiEditorFactory editorFactory);
        HResult CreateInstance(Guid guid, out object instance);
    }
}
