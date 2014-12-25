using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiOpenDocumentManager
    {
        HResult IsDocumentOpen(string document, out INiHierarchy hier, out INiWindowFrame windowFrame);
        HResult OpenStandardEditor(Guid? editorGuid, string document, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame);
        HResult OpenSpecificEditor(string document, Guid editorType, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame);
    }
}
