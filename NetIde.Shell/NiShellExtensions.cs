using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiShellExtensions
    {
        public static IEnumerable<INiWindowFrame> GetDocumentWindows(this INiShell self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            INiIterator<INiWindowFrame> iterator;
            ErrorUtil.ThrowOnFailure(self.GetDocumentWindowIterator(out iterator));

            return iterator.GetEnumerable();
        }
    }
}
