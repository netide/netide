using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiHierarchyExtensions
    {
        public static object GetPropertyEx(this INiHierarchy self, int property)
        {
            object result;
            ErrorUtil.ThrowOnFailure(self.GetProperty(property, out result));

            return result;
        }

        public static object GetPropertyEx(this INiHierarchy self, NiHierarchyProperty property)
        {
            return GetPropertyEx(self, (int)property);
        }

        public static void SetPropertyEx(this INiHierarchy self, int property, object value)
        {
            ErrorUtil.ThrowOnFailure(self.SetProperty(property, value));
        }

        public static void SetPropertyEx(this INiHierarchy self, NiHierarchyProperty property, object value)
        {
            SetPropertyEx(self, (int)property, value);
        }

        public static IEnumerable<INiHierarchy> GetChildren(this INiHierarchy self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            var child = (INiHierarchy)self.GetPropertyEx(NiHierarchyProperty.FirstChild);

            while (child != null)
            {
                yield return child;

                child = (INiHierarchy)child.GetPropertyEx(NiHierarchyProperty.NextSibling);
            }
        }
    }
}
