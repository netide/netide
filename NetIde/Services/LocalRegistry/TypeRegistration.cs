using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.LocalRegistry
{
    internal class TypeRegistration : ITypeRegistration
    {
        public Guid Package { get; private set; }
        public Guid Id { get; private set; }
        public string Type { get; private set; }

        public TypeRegistration(Guid package, Guid id, string type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Package = package;
            Id = id;
            Type = type;
        }
    }
}
