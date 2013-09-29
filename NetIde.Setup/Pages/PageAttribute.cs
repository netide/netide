using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Setup.Pages
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class PageAttribute : Attribute
    {
        public Type Type { get; private set; }

        public PageAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Type = type;
        }
    }
}
