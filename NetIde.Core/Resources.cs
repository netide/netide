using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell;
using NetIde.Shell.Interop;

namespace NetIde.Core
{
    internal static class Resources
    {
        public static IResource Exit = GetResource("Exit.png");
        public static IResource Folders = GetResource("folders.ico");

        private static IResource GetResource(string resourceKey)
        {
            return ResourceUtil.FromManifestResourceStream(
                typeof(Resources).Assembly,
                typeof(Resources).Namespace + ".Resources." + resourceKey
            );
        }
    }
}
