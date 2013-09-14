using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Test.Support
{
    internal static class EnumExtensions
    {
        public static string GetDescription(this Enum self)
        {
            if (self == null)
                throw new ArgumentNullException("self");

            var attribute = self.GetType().GetField(self.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);

            if (attribute.Length > 0)
                return ((System.ComponentModel.DescriptionAttribute)attribute[0]).Description;

            return null;
        }
    }
}
