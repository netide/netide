using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal class EditorFactoryRegistration : IRegistration
    {
        public Guid Package { get; private set; }
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }

        public EditorFactoryRegistration(Guid package, Guid id, string displayName)
        {
            if (displayName == null)
                throw new ArgumentNullException("displayName");

            Package = package;
            Id = id;
            DisplayName = displayName;
        }
    }
}
