using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public static class ArrayUtil
    {
        public static bool Equals(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (a == null || b == null || a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        public static T[] GetEmptyArray<T>()
        {
            return EmptyArrayProvider<T>.EmptyArray;
        }

        private class EmptyArrayProvider<T>
        {
            public static readonly T[] EmptyArray = new T[0];
        }
    }
}
