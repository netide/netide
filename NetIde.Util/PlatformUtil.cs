using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string GetNewline(LineTermination mode)
        {
            switch (mode)
            {
                case LineTermination.Pc:
                    return "\r\n";

                case LineTermination.Unix:
                    return "\n";

                case LineTermination.Mac:
                    return "\r";

                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        public static string NormalizeLineTermination(string value, LineTermination mode)
        {
            if (value == null)
                return null;

            var sb = new StringBuilder();

            NormalizeLineTermination(sb, value, mode);

            return sb.ToString();
        }

        public static void NormalizeLineTermination(StringBuilder target, string value, LineTermination mode)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            if (value == null)
                return;

            string lineTermination = GetNewline(mode);

            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                switch (c)
                {
                    case '\r':
                        if (i < value.Length - 1 && value[i + 1] == '\n')
                            i++;
                        target.Append(lineTermination);
                        break;

                    case '\n':
                        target.Append(lineTermination);
                        break;

                    default:
                        target.Append(c);
                        break;
                }
            }
        }

        public static string GetLineTermination(LineTermination mode)
        {
            switch (mode)
            {
                case LineTermination.Pc:
                    return "\r\n";
                case LineTermination.Unix:
                    return "\n";
                case LineTermination.Mac:
                    return "\r";
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }
    }
}
