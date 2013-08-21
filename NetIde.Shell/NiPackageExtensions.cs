using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiPackageExtensions
    {
        public static string ResolveStringResource(this INiPackage self, string key)
        {
            if (String.IsNullOrEmpty(key))
                return key;

            if (key.StartsWith("@"))
            {
                string value;
                ErrorUtil.ThrowOnFailure(self.GetStringResource(key.Substring(1), out value));
                return value;
            }

            return key;
        }
    }
}
