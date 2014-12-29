using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public static class CollectionExtensions
    {
        public static void AddRange(this IList self, IEnumerable items)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                self.Add(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
        {
            if (self == null)
                throw new ArgumentNullException("self");
            if (items == null)
                throw new ArgumentNullException("items");

            foreach (var item in items)
            {
                self.Add(item);
            }
        }
    }
}
