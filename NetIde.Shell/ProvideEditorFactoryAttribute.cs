using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideEditorFactoryAttribute : RegistrationAttribute
    {
        public Type EditorType { get; private set; }
        public string Name { get; private set; }

        public ProvideEditorFactoryAttribute(Type editorType, string name)
        {
            if (editorType == null)
                throw new ArgumentNullException("editorType");
            if (name == null)
                throw new ArgumentNullException("name");

            EditorType = editorType;
            Name = name;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            string editorGuid = EditorType.GUID.ToString("B").ToUpperInvariant();

            using (var key = packageKey.CreateSubKey("Editors\\" + editorGuid))
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
