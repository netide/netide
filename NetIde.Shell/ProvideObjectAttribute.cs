using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProvideObjectAttribute : RegistrationAttribute
    {
        public Type Type { get; private set; }

        public ProvideObjectAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Type = type;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string guid = Type.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("CLSID\\" + guid))
            {
                key.SetValue("Class", Type.AssemblyQualifiedName);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
