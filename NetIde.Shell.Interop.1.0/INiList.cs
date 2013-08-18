using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiList<T> : IEnumerable<T>
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;

        int Count { get; }

        T this[int index] { get; }

        HResult Add(T item);

        HResult Insert(int index, T item);

        HResult Remove(T item);
    }
}
