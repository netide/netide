using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetIde.Util.Forms
{
    internal static class AssemblyInformation
    {
        private static readonly byte[][] _microsoftAssemblies =
        {
            new byte[] { 0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35 },
            new byte[] { 0xb0, 0x3f, 0x5f, 0x7f, 0x11, 0xd5, 0x0a, 0x3a },
            new byte[] { 0xb7, 0x7a, 0x5c, 0x56, 0x19, 0x34, 0xe0, 0x89 }
        };

        public static string GetProductTitle(Assembly assembly)
        {
            var attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), true);

            if (attributes != null && attributes.Length > 0)
                return ((AssemblyTitleAttribute)attributes[0]).Title;

            return assembly.GetName().Name;
        }

        public static Assembly FindEntryAssembly()
        {
            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly == null)
            {
                // HACK from http://stackoverflow.com/questions/758077.
                // Walk the stack trace for the last frame that is from
                // a non Microsoft assembly.

                foreach (var frame in new StackTrace().GetFrames())
                {
                    var frameAssembly = frame.GetMethod().DeclaringType.Assembly;

                    if (!IsMicrosoftAssembly(frameAssembly))
                        entryAssembly = frameAssembly;
                }
            }

            return entryAssembly;
        }
        
        public static bool IsMicrosoftAssembly(this Assembly assembly)
        {
            return IsKnownAssembly(assembly, _microsoftAssemblies);
        }

        private static bool IsKnownAssembly(Assembly assembly, byte[][] knownTokens)
        {
            var token = assembly.GetName().GetPublicKeyToken();

            foreach (var microsoftToken in knownTokens)
            {
                if (ArrayEquals(token, microsoftToken))
                    return true;
            }

            return false;
        }

        private static bool ArrayEquals(byte[] a, byte[] b)
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
    }
}
