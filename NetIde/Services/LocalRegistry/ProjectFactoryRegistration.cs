using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal class ProjectFactoryRegistration : IRegistration
    {
        public Guid Package { get; private set; }
        public Guid Id { get; private set; }
        public string DisplayName { get; private set; }
        public string ProjectFileExtensions { get; private set; }
        public string DefaultProjectExtension { get; private set; }
        public string PossibleProjectExtensions { get; private set; }

        public ProjectFactoryRegistration(Guid package, Guid id, string displayName, string projectFileExtensions, string defaultProjectExtension, string possibleProjectExtensions)
        {
            if (displayName == null)
                throw new ArgumentNullException("displayName");
            if (projectFileExtensions == null)
                throw new ArgumentNullException("projectFileExtensions");
            if (defaultProjectExtension == null)
                throw new ArgumentNullException("defaultProjectExtension");
            if (possibleProjectExtensions == null)
                throw new ArgumentNullException("possibleProjectExtensions");

            Package = package;
            Id = id;
            DisplayName = displayName;
            ProjectFileExtensions = projectFileExtensions;
            DefaultProjectExtension = defaultProjectExtension;
            PossibleProjectExtensions = possibleProjectExtensions;
        }
    }
}
