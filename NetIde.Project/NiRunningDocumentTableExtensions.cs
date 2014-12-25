using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Project.Interop;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Project
{
    public static class NiRunningDocumentTableExtensions
    {
        public static IEnumerable<int> GetDocuments(this INiRunningDocumentTable self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            INiIterator<int> iterator;
            ErrorUtil.ThrowOnFailure(self.GetDocumentIterator(out iterator));

            return iterator.GetEnumerable();
        }
    }
}
