using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideProjectFactoryAttribute : RegistrationAttribute
    {
        public Type FactoryType { get; private set; }
        public string Name { get; private set; }
        public string ProjectFileExtensions { get; private set; }
        public string DefaultProjectExtension { get; private set; }
        public string PossibleProjectExtensions { get; private set; }

        public ProvideProjectFactoryAttribute(Type factoryType, string name, string projectFileExtensions, string defaultProjectExtension, string possibleProjectExtensions)
        {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (name == null)
                throw new ArgumentNullException("name");
            if (projectFileExtensions == null)
                throw new ArgumentNullException("projectFileExtensions");
            if (defaultProjectExtension == null)
                throw new ArgumentNullException("defaultProjectExtension");
            if (possibleProjectExtensions == null)
                throw new ArgumentNullException("possibleProjectExtensions");

            FactoryType = factoryType;
            Name = name;
            ProjectFileExtensions = projectFileExtensions;
            DefaultProjectExtension = defaultProjectExtension;
            PossibleProjectExtensions = possibleProjectExtensions;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string factoryGuid = FactoryType.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("Projects\\" + factoryGuid))
            {
                key.SetValue(null, ResolveStringResource(package, Name));
                key.SetValue("DisplayName", Name);
                key.SetValue("ProjectFileExtensions", ProjectFileExtensions);
                key.SetValue("DefaultProjectExtension", DefaultProjectExtension);
                key.SetValue("PossibleProjectExtensions", PossibleProjectExtensions);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
