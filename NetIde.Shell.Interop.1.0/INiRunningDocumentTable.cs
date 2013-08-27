using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiRunningDocumentTable
    {
        HResult GetDocumentIterator(out INiRunningDocumentIterator iterator);
        HResult Register(string document, INiHierarchy hier, INiPersistDocData docData, out int cookie);
        HResult Unregister(int cookie);
        HResult GetDocumentInfo(int cookie, out string document, out INiHierarchy hier, out INiPersistDocData docData);
    }
}
