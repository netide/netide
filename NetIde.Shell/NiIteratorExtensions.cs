using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiIteratorExtensions
    {
        public static IEnumerable<T> GetEnumerable<T>(this INiIterator<T> self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            using (self)
            {
                while (true)
                {
                    bool available;
                    ErrorUtil.ThrowOnFailure(self.Next(out available));

                    if (!available)
                        break;

                    T current;
                    ErrorUtil.ThrowOnFailure(self.GetCurrent(out current));

                    yield return current;
                }
            }
        }
    }
}
