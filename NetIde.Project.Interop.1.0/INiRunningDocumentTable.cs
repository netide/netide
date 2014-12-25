using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiRunningDocumentTable
    {
        HResult GetDocumentIterator(out INiIterator<int> iterator);
        HResult Register(string document, INiHierarchy hier, INiPersistDocData docData, out int cookie);
        HResult Unregister(int cookie);
        HResult GetDocumentInfo(int cookie, out string document, out INiHierarchy hier, out INiPersistDocData docData);
    }
}
