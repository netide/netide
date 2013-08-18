using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiList<T> : ServiceObject, INiList<T>
    {
        private readonly System.Collections.ObjectModel.ObservableCollection<T> _inner = new System.Collections.ObjectModel.ObservableCollection<T>();

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _inner.CollectionChanged += value; }
            remove { _inner.CollectionChanged -= value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            // When enumerating, we always create a copy of the collection.
            // This prevents us from returning a MBRO and prevents exceptions
            // because of changes in the collection.

            return ((IEnumerable<T>)_inner.ToArray()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _inner.Count)
                    return default(T);

                return _inner[index];
            }
        }

        public int Count
        {
            get { return _inner.Count; }
        }

        public HResult Add(T item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                _inner.Add(item);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Insert(int index, T item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                _inner.Insert(index, item);

                return HResult.OK;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult Remove(T item)
        {
            try
            {
                if (item == null)
                    throw new ArgumentNullException("item");

                bool removed = _inner.Remove(item);

                return removed ? HResult.OK : HResult.False;
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }
    }
}
