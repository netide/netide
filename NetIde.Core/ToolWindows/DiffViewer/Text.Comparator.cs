using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Diff;
using NGit.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    partial class Text
    {
        public abstract class Comparator : SequenceComparator<Text>
        {
            private sealed class _RawTextComparator_56 : Comparator
            {
                public _RawTextComparator_56()
                {
                }

                public override bool Equals(Text a, int ai, Text b, int bi)
                {
                    ai++;
                    bi++;
                    int @as = a._lines.Get(ai);
                    int bs = b._lines.Get(bi);
                    int ae = a._lines.Get(ai + 1);
                    int be = b._lines.Get(bi + 1);
                    if (ae - @as != be - bs)
                    {
                        return false;
                    }
                    while (@as < ae)
                    {
                        if (a.Content[@as++] != b.Content[bs++])
                        {
                            return false;
                        }
                    }
                    return true;
                }

                protected internal override int HashRegion(string raw, int ptr, int end)
                {
                    int hash = 5381;
                    for (; ptr < end; ptr++)
                    {
                        hash = ((hash << 5) + hash) + (raw[ptr] & unchecked((int)(0xff)));
                    }
                    return hash;
                }
            }

            /// <summary>No special treatment.</summary>
            /// <remarks>No special treatment.</remarks>
            public static readonly Comparator DEFAULT = new _RawTextComparator_56();

            private sealed class _RawTextComparator_87 : Comparator
            {
                public _RawTextComparator_87()
                {
                }

                public override bool Equals(Text a, int ai, Text b, int bi)
                {
                    ai++;
                    bi++;
                    int @as = a._lines.Get(ai);
                    int bs = b._lines.Get(bi);
                    int ae = a._lines.Get(ai + 1);
                    int be = b._lines.Get(bi + 1);
                    ae = RawCharUtil.TrimTrailingWhitespace(a.Content, @as, ae);
                    be = RawCharUtil.TrimTrailingWhitespace(b.Content, bs, be);
                    while (@as < ae && bs < be)
                    {
                        char ac = a.Content[@as];
                        char bc = b.Content[bs];
                        while (@as < ae - 1 && RawCharUtil.IsWhitespace(ac))
                        {
                            @as++;
                            ac = a.Content[@as];
                        }
                        while (bs < be - 1 && RawCharUtil.IsWhitespace(bc))
                        {
                            bs++;
                            bc = b.Content[bs];
                        }
                        if (ac != bc)
                        {
                            return false;
                        }
                        @as++;
                        bs++;
                    }
                    return @as == ae && bs == be;
                }

                protected internal override int HashRegion(string raw, int ptr, int end)
                {
                    int hash = 5381;
                    for (; ptr < end; ptr++)
                    {
                        char c = raw[ptr];
                        if (!RawCharUtil.IsWhitespace(c))
                        {
                            hash = ((hash << 5) + hash) + (c & unchecked((int)(0xff)));
                        }
                    }
                    return hash;
                }
            }

            /// <summary>Ignores all whitespace.</summary>
            /// <remarks>Ignores all whitespace.</remarks>
            public static readonly Comparator WS_IGNORE_ALL = new _RawTextComparator_87
                ();

            private sealed class _RawTextComparator_138 : Comparator
            {
                public _RawTextComparator_138()
                {
                }

                public override bool Equals(Text a, int ai, Text b, int bi)
                {
                    ai++;
                    bi++;
                    int @as = a._lines.Get(ai);
                    int bs = b._lines.Get(bi);
                    int ae = a._lines.Get(ai + 1);
                    int be = b._lines.Get(bi + 1);
                    @as = RawCharUtil.TrimLeadingWhitespace(a.Content, @as, ae);
                    bs = RawCharUtil.TrimLeadingWhitespace(b.Content, bs, be);
                    if (ae - @as != be - bs)
                    {
                        return false;
                    }
                    while (@as < ae)
                    {
                        if (a.Content[@as++] != b.Content[bs++])
                        {
                            return false;
                        }
                    }
                    return true;
                }

                protected internal override int HashRegion(string raw, int ptr, int end)
                {
                    int hash = 5381;
                    ptr = RawCharUtil.TrimLeadingWhitespace(raw, ptr, end);
                    for (; ptr < end; ptr++)
                    {
                        hash = ((hash << 5) + hash) + (raw[ptr] & unchecked((int)(0xff)));
                    }
                    return hash;
                }
            }

            /// <summary>Ignores leading whitespace.</summary>
            /// <remarks>Ignores leading whitespace.</remarks>
            public static readonly Comparator WS_IGNORE_LEADING = new _RawTextComparator_138
                ();

            private sealed class _RawTextComparator_173 : Comparator
            {
                public _RawTextComparator_173()
                {
                }

                public override bool Equals(Text a, int ai, Text b, int bi)
                {
                    ai++;
                    bi++;
                    int @as = a._lines.Get(ai);
                    int bs = b._lines.Get(bi);
                    int ae = a._lines.Get(ai + 1);
                    int be = b._lines.Get(bi + 1);
                    ae = RawCharUtil.TrimTrailingWhitespace(a.Content, @as, ae);
                    be = RawCharUtil.TrimTrailingWhitespace(b.Content, bs, be);
                    if (ae - @as != be - bs)
                    {
                        return false;
                    }
                    while (@as < ae)
                    {
                        if (a.Content[@as++] != b.Content[bs++])
                        {
                            return false;
                        }
                    }
                    return true;
                }

                protected internal override int HashRegion(string raw, int ptr, int end)
                {
                    int hash = 5381;
                    end = RawCharUtil.TrimTrailingWhitespace(raw, ptr, end);
                    for (; ptr < end; ptr++)
                    {
                        hash = ((hash << 5) + hash) + (raw[ptr] & unchecked((int)(0xff)));
                    }
                    return hash;
                }
            }

            /// <summary>Ignores trailing whitespace.</summary>
            /// <remarks>Ignores trailing whitespace.</remarks>
            public static readonly Comparator WS_IGNORE_TRAILING = new _RawTextComparator_173
                ();

            private sealed class _RawTextComparator_208 : Comparator
            {
                public _RawTextComparator_208()
                {
                }

                public override bool Equals(Text a, int ai, Text b, int bi)
                {
                    ai++;
                    bi++;
                    int @as = a._lines.Get(ai);
                    int bs = b._lines.Get(bi);
                    int ae = a._lines.Get(ai + 1);
                    int be = b._lines.Get(bi + 1);
                    ae = RawCharUtil.TrimTrailingWhitespace(a.Content, @as, ae);
                    be = RawCharUtil.TrimTrailingWhitespace(b.Content, bs, be);
                    while (@as < ae && bs < be)
                    {
                        char ac = a.Content[@as];
                        char bc = b.Content[bs];
                        if (ac != bc)
                        {
                            return false;
                        }
                        if (RawCharUtil.IsWhitespace(ac))
                        {
                            @as = RawCharUtil.TrimLeadingWhitespace(a.Content, @as, ae);
                        }
                        else
                        {
                            @as++;
                        }
                        if (RawCharUtil.IsWhitespace(bc))
                        {
                            bs = RawCharUtil.TrimLeadingWhitespace(b.Content, bs, be);
                        }
                        else
                        {
                            bs++;
                        }
                    }
                    return @as == ae && bs == be;
                }

                protected internal override int HashRegion(string raw, int ptr, int end)
                {
                    int hash = 5381;
                    end = RawCharUtil.TrimTrailingWhitespace(raw, ptr, end);
                    while (ptr < end)
                    {
                        char c = raw[ptr];
                        hash = ((hash << 5) + hash) + (c & unchecked((int)(0xff)));
                        if (RawCharUtil.IsWhitespace(c))
                        {
                            ptr = RawCharUtil.TrimLeadingWhitespace(raw, ptr, end);
                        }
                        else
                        {
                            ptr++;
                        }
                    }
                    return hash;
                }
            }

            /// <summary>Ignores whitespace occurring between non-whitespace characters.</summary>
            /// <remarks>Ignores whitespace occurring between non-whitespace characters.</remarks>
            public static readonly Comparator WS_IGNORE_CHANGE = new _RawTextComparator_208
                ();

            public override int Hash(Text seq, int lno)
            {
                int begin = seq._lines.Get(lno + 1);
                int end = seq._lines.Get(lno + 2);
                return HashRegion(seq.Content, begin, end);
            }

            public override Edit ReduceCommonStartEnd(Text a, Text b, Edit e)
            {
                // This is a faster exact match based form that tries to improve
                // performance for the common case of the header and trailer of
                // a text file not changing at all. After this fast path we use
                // the slower path based on the super class' using equals() to
                // allow for whitespace ignore modes to still work.
                if (e.GetBeginA() == e.GetEndA() || e.GetBeginB() == e.GetEndB())
                {
                    return e;
                }
                string aRaw = a.Content;
                string bRaw = b.Content;
                int aPtr = a._lines.Get(e.GetBeginA() + 1);
                int bPtr = a._lines.Get(e.GetBeginB() + 1);
                int aEnd = a._lines.Get(e.GetEndA() + 1);
                int bEnd = b._lines.Get(e.GetEndB() + 1);
                // This can never happen, but the JIT doesn't know that. If we
                // define this assertion before the tight while loops below it
                // should be able to skip the array bound checks on access.
                //
                if (aPtr < 0 || bPtr < 0 || aEnd > aRaw.Length || bEnd > bRaw.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                while (aPtr < aEnd && bPtr < bEnd && aRaw[aPtr] == bRaw[bPtr])
                {
                    aPtr++;
                    bPtr++;
                }
                while (aPtr < aEnd && bPtr < bEnd && aRaw[aEnd - 1] == bRaw[bEnd - 1])
                {
                    aEnd--;
                    bEnd--;
                }
                int newBeginA = FindForwardLine(a._lines, e.GetBeginA(), aPtr);
                int newBeginB = FindForwardLine(b._lines, e.GetBeginB(), bPtr);
                int newEndA = FindReverseLine(a._lines, e.GetEndA(), aEnd);
                bool partialA = aEnd < a._lines.Get(newEndA + 1);
                if (partialA)
                {
                    bEnd += a._lines.Get(newEndA + 1) - aEnd;
                }
                int newEndB = FindReverseLine(b._lines, e.GetEndB(), bEnd);
                if (!partialA && bEnd < b._lines.Get(newEndB + 1))
                {
                    newEndB++;
                }
                return base.ReduceCommonStartEnd(a, b, new Edit(newBeginA, newEndA, newBeginB, newEndB));
            }

            private static int FindForwardLine(IntList lines, int idx, int ptr)
            {
                int end = lines.Size() - 2;
                while (idx < end && lines.Get(idx + 2) < ptr)
                {
                    idx++;
                }
                return idx;
            }

            private static int FindReverseLine(IntList lines, int idx, int ptr)
            {
                while (0 < idx && ptr <= lines.Get(idx))
                {
                    idx--;
                }
                return idx;
            }

            /// <summary>Compute a hash code for a region.</summary>
            /// <remarks>Compute a hash code for a region.</remarks>
            /// <param name="raw">the raw file content.</param>
            /// <param name="ptr">first char of the region to hash.</param>
            /// <param name="end">1 past the last char of the region.</param>
            /// <returns>hash code for the region <code>[ptr, end)</code> of raw.</returns>
            protected internal abstract int HashRegion(string raw, int ptr, int end);
        }

        private class RawCharUtil
        {
            private static readonly bool[] WHITESPACE = new bool[256];

            static RawCharUtil()
            {
                WHITESPACE['\r'] = true;
                WHITESPACE['\n'] = true;
                WHITESPACE['\t'] = true;
                WHITESPACE[' '] = true;
            }

            /// <summary>Determine if an 8-bit US-ASCII encoded character is represents whitespace
            /// 	</summary>
            /// <param name="c">the 8-bit US-ASCII encoded character</param>
            /// <returns>true if c represents a whitespace character in 8-bit US-ASCII</returns>
            public static bool IsWhitespace(char c)
            {
                return WHITESPACE[c & unchecked((int)(0xff))];
            }

            /// <summary>
            /// Returns the new end point for the char array passed in after trimming any
            /// trailing whitespace characters, as determined by the isWhitespace()
            /// function.
            /// </summary>
            /// <remarks>
            /// Returns the new end point for the char array passed in after trimming any
            /// trailing whitespace characters, as determined by the isWhitespace()
            /// function. start and end are assumed to be within the bounds of raw.
            /// </remarks>
            /// <param name="raw">the char array containing the portion to trim whitespace for</param>
            /// <param name="start">the start of the section of bytes</param>
            /// <param name="end">the end of the section of bytes</param>
            /// <returns>the new end point</returns>
            public static int TrimTrailingWhitespace(string raw, int start, int end)
            {
                int ptr = end - 1;
                while (start <= ptr && IsWhitespace(raw[ptr]))
                {
                    ptr--;
                }
                return ptr + 1;
            }

            /// <summary>
            /// Returns the new start point for the char array passed in after trimming
            /// any leading whitespace characters, as determined by the isWhitespace()
            /// function.
            /// </summary>
            /// <remarks>
            /// Returns the new start point for the char array passed in after trimming
            /// any leading whitespace characters, as determined by the isWhitespace()
            /// function. start and end are assumed to be within the bounds of raw.
            /// </remarks>
            /// <param name="raw">the char array containing the portion to trim whitespace for</param>
            /// <param name="start">the start of the section of bytes</param>
            /// <param name="end">the end of the section of bytes</param>
            /// <returns>the new start point</returns>
            public static int TrimLeadingWhitespace(string raw, int start, int end)
            {
                while (start < end && IsWhitespace(raw[start]))
                {
                    start++;
                }
                return start;
            }

            public RawCharUtil()
            {
            }
            // This will never be called
        }

    }
}
