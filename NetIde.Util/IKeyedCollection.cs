using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public interface IKeyedCollection<in TKey, TItem> : IList<TItem>
    {
        IEqualityComparer<TKey> Comparer { get; }

        TItem this[TKey key] { get; }

        bool Contains(TKey key);

        bool Remove(TKey key);

        bool TryGetValue(TKey key, out TItem item);
    }
}
