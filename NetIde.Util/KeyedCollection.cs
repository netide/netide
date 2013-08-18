using System;
using System.Collections.Generic;
using System.Text;

namespace NetIde.Util
{
    [Serializable]
    public abstract class KeyedCollection<TKey, TItem> : System.Collections.ObjectModel.KeyedCollection<TKey, TItem>, IKeyedCollection<TKey, TItem>
    {
        protected KeyedCollection()
        {
        }

        protected KeyedCollection(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
        }

        protected KeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
            : base(comparer, dictionaryCreationThreshold)
        {
        }

        public bool TryGetValue(TKey key, out TItem item)
        {
            if (Contains(key))
            {
                item = this[key];
                return true;
            }
            else
            {
                item = default(TItem);
                return false;
            }
        }
    }

    public static class KeyedCollection
    {
        public static IKeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keySelector)
        {
            return new AutoKeyedCollection<TKey, TItem>(keySelector);
        }

        public static IKeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return new AutoKeyedCollection<TKey, TItem>(keySelector, comparer);
        }

        public static IKeyedCollection<TKey, TItem> Create<TKey, TItem>(Func<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
        {
            return new AutoKeyedCollection<TKey, TItem>(keySelector, comparer, dictionaryCreationThreshold);
        }

        private class AutoKeyedCollection<TKey, TItem> : KeyedCollection<TKey, TItem>
        {
            private readonly Func<TItem, TKey> _keySelector;

            public AutoKeyedCollection(Func<TItem, TKey> keySelector)
            {
                if (keySelector == null)
                    throw new ArgumentNullException("keySelector");

                _keySelector = keySelector;
            }

            public AutoKeyedCollection(Func<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer)
                : base(comparer)
            {
                if (keySelector == null)
                    throw new ArgumentNullException("keySelector");

                _keySelector = keySelector;
            }

            public AutoKeyedCollection(Func<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold)
                : base(comparer, dictionaryCreationThreshold)
            {
                if (keySelector == null)
                    throw new ArgumentNullException("keySelector");

                _keySelector = keySelector;
            }

            protected override TKey GetKeyForItem(TItem item)
            {
                return _keySelector(item);
            }
        }
    }
}
