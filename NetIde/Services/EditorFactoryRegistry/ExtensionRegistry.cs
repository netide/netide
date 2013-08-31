using System.Text;
using System.Collections.Generic;
using System;
using NetIde.Util;

namespace NetIde.Services.EditorFactoryRegistry
{
    internal class ExtensionRegistry : KeyedCollection<string, ExtensionRegistration>
    {
        public ExtensionRegistry()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        protected override string GetKeyForItem(ExtensionRegistration item)
        {
            return item.Extension;
        }
    }
}