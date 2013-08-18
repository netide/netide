using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Shell.Interop;

namespace NetIde
{
    internal static class RegistryUtil
    {
        public static void ForEachPackage(IServiceProvider serviceProvider, Action<Guid, RegistryKey> callback)
        {
            ForEachPackage(serviceProvider, null, callback);
        }

        public static void ForEachPackage(IServiceProvider serviceProvider, string subKeyName, Action<Guid, RegistryKey> callback)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException("serviceProvider");

            var env = (INiEnv)serviceProvider.GetService(typeof(INiEnv));
            string context = env.Context;

            using (var key = Registry.CurrentUser.OpenSubKey("Software\\Net IDE\\" + context + "\\Packages"))
            {
                foreach (string packageId in key.GetSubKeyNames())
                {
                    string packageSubKeyName = packageId;

                    if (subKeyName != null)
                        packageSubKeyName += "\\" + subKeyName;

                    using (var packageKey = key.OpenSubKey(packageSubKeyName))
                    {
                        if (packageKey != null)
                            callback(Guid.Parse(packageId), packageKey);
                    }
                }
            }
        }

        public static bool GetBool(object value)
        {
            return (int)value != 0;
        }
    }
}
