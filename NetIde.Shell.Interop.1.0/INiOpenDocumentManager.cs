using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiOpenDocumentManager
    {
        HResult GetStandardEditorFactory(Guid? editorGuid, string document, out INiEditorFactory editorFactory);
        HResult IsDocumentOpen(string document, out INiHierarchy hier, out INiWindowFrame windowFrame);
        HResult OpenStandardEditor(Guid? editorGuid, string document, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame);
        HResult OpenSpecificEditor(string document, Guid editorType, INiHierarchy hier, IServiceProvider serviceProvider, out INiWindowFrame windowFrame);
    }
}
