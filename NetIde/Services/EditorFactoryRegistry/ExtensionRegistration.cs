using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Services.EditorFactoryRegistry
{
    internal class ExtensionRegistration
    {
        public Guid FactoryType { get; private set; }
        public string Extension { get; private set; }
        public int Priority { get; private set; }
        public string DefaultName { get; private set; }
        public string TemplateResource { get; private set; }

        public ExtensionRegistration(Guid factoryType, string extension, int priority, string defaultName, string templateResource)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            FactoryType = factoryType;
            Extension = extension;
            Priority = priority;
            DefaultName = defaultName;
            TemplateResource = templateResource;
        }
    }
}