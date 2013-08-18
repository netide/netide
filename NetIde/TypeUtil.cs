using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NetIde.Shell;

namespace NetIde
{
    internal static class TypeUtil
    {
        public static Type ResolveType(string typeName)
        {
            return ResolveType(typeName, true);
        }

        public static Type ResolveType(string typeName, bool throwOnError)
        {
            if (typeName == null)
                throw new ArgumentNullException("typeName");

            int pos = typeName.IndexOf(',');
            Assembly assembly;

            if (pos == -1)
            {
                assembly = typeof(TypeUtil).Assembly;
            }
            else
            {
                string assemblyName = typeName.Substring(pos + 1).Trim();
                typeName = typeName.Substring(0, pos).Trim();

                assembly = Assembly.Load(assemblyName);
            }


            return assembly.GetType(typeName, throwOnError);
        }
    }
}
