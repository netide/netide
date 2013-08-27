using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiRunningDocumentTableExtensions
    {
        public static IEnumerable<int> GetDocuments(this INiRunningDocumentTable self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            INiRunningDocumentIterator iterator;
            ErrorUtil.ThrowOnFailure(self.GetDocumentIterator(out iterator));

            using (iterator)
            {
                while (true)
                {
                    bool available;
                    ErrorUtil.ThrowOnFailure(iterator.Next(out available));

                    if (!available)
                        break;

                    int cookie;
                    ErrorUtil.ThrowOnFailure(iterator.GetCurrent(out cookie));

                    yield return cookie;
                }
            }
        }
    }
}
