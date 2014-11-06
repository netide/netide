using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Util
{
    public static class PlatformUtil
    {
        public static readonly bool IsMono = Type.GetType("Mono.Runtime") != null;

        public static readonly bool IsUnix = DetectUnix();

        public static readonly bool IsWindows = !IsUnix;

        private static bool DetectUnix()
        {
            int p = (int)Environment.OSVersion.Platform;

            return (p == 4) || (p == 6) || (p == 128);
        }

        public static readonly LineTermination NativeLineTermination = GetNativeLineTermination();

        private static LineTermination GetNativeLineTermination()
        {
            switch (Environment.NewLine)
            {
                case "\r": return LineTermination.Mac;
                case "\n": return LineTermination.Unix;
                default: return LineTermination.Pc;
            }
        }
    }
}
