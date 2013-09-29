using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> values)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (values == null)
                throw new ArgumentNullException("values");

            foreach (var value in values)
            {
                self.Add(value);
            }
        }
    }
}
