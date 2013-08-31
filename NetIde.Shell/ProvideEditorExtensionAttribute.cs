using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideEditorExtensionAttribute : RegistrationAttribute
    {
        public Type FactoryType { get; private set; }
        public string Extension { get; private set; }
        public int Priority { get; private set; }
        public Guid? ProjectGuid { get; set; }
        public string DefaultName { get; set; }
        public string TemplateResource { get; set; }

        public ProvideEditorExtensionAttribute(Type factoryType, string extension, int priority)
        {
            if (factoryType == null)
                throw new ArgumentNullException("factoryType");
            if (extension == null)
                throw new ArgumentNullException("extension");

            FactoryType = factoryType;
            Extension = extension;
            Priority = priority;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string keyName;

            if (ProjectGuid.HasValue)
                keyName = "Projects\\" + ProjectGuid.Value.ToString("B").ToUpperInvariant() + "\\Extensions\\" + Extension;
            else
                keyName = "Extensions\\" + Extension;

            using (var key = packageKey.CreateSubKey(keyName))
            {
                if (DefaultName != null)
                {
                    key.SetValue(null, ResolveStringResource(package, DefaultName));
                    key.SetValue("DefaultName", DefaultName);
                }

                key.SetValue("FactoryType", FactoryType.GUID.ToString("B").ToUpperInvariant());
                key.SetValue("Priority", Priority);
                
                if (TemplateResource != null)
                    key.SetValue("TemplateResource", TemplateResource);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
