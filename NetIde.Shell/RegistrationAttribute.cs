using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class RegistrationAttribute : Attribute
    {
        public abstract void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey);

        public abstract void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey);

        protected string ResolveStringResource(INiPackage package, string key)
        {
            if (package == null)
                throw new ArgumentNullException("package");
            if (key == null)
                throw new ArgumentNullException("key");

            if (key.StartsWith("@"))
            {
                string value;
                ErrorUtil.ThrowOnFailure(package.GetStringResource(key.Substring(1), out value));
                return value;
            }

            return key;
        }
    }
}
