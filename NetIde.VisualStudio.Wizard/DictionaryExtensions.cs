using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetIde.VisualStudio.Wizard
{
    internal static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key)
        {
            return GetValueOrDefault(self, key, default(TValue));
        }

        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> self, TKey key, TValue value)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            TValue result;

            if (self.TryGetValue(key, out result))
                return result;

            return value;
        }
    }
}
