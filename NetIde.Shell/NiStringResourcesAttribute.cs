using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class NiStringResourcesAttribute : Attribute
    {
        public string ResourceName { get; private set; }

        public NiStringResourcesAttribute(string resourceName)
        {
            if (resourceName == null)
                throw new ArgumentNullException("resourceName");

            ResourceName = resourceName;
        }
    }
}
