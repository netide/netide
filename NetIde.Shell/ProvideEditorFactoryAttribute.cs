using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideEditorFactoryAttribute : RegistrationAttribute
    {
        public Type FactoryType { get; private set; }
        public string Name { get; private set; }

        public ProvideEditorFactoryAttribute(Type factoryType, string name)
        {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (name == null)
                throw new ArgumentNullException("name");

            FactoryType = factoryType;
            Name = name;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string factoryGuid = FactoryType.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("Editors\\" + factoryGuid))
            {
                key.SetValue(null, ResolveStringResource(package, Name));
                key.SetValue("DisplayName", Name);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
