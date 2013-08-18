using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NiResourcesAttribute : Attribute
    {
        public string ResourceName { get; private set; }

        public NiResourcesAttribute(string resourceName)
        {
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");

            ResourceName = resourceName;
        }
    }
}
