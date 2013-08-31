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
        public static IResource DiskBlue = GetResource("DiskBlue.png");
        public static IResource Disks = GetResource("Disks.png");
        public static IResource Documents = GetResource("Documents.png");
        public static IResource DocumentsNew = GetResource("DocumentsNew.png");
        public static IResource Find = GetResource("Find.png");
        public static IResource Replace = GetResource("Replace.png");

        private static IResource GetResource(string resourceKey)
        {
            return ResourceUtil.FromManifestResourceStream(
                typeof(Resources).Assembly,
                typeof(Resources).Namespace + ".Resources." + resourceKey
            );
        }
    }
}
