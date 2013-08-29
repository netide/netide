using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public static class NiWindowFrameExtensions
    {
        public static object GetPropertyEx(this INiWindowFrame self, int property)
        {
            object result;
            ErrorUtil.ThrowOnFailure(self.GetProperty(property, out result));

            return result;
        }

        public static object GetPropertyEx(this INiWindowFrame self, NiFrameProperty property)
        {
            return GetPropertyEx(self, (int)property);
        }

        public static void SetPropertyEx(this INiWindowFrame self, int property, object value)
        {
            ErrorUtil.ThrowOnFailure(self.SetProperty(property, value));
        }

        public static void SetPropertyEx(this INiWindowFrame self, NiFrameProperty property, object value)
        {
            SetPropertyEx(self, (int)property, value);
        }
    }
}
