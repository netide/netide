using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Settings
{
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class SettingsAttribute : Attribute
    {
        public Type SettingsType { get; private set; }
        public string Group { get; private set; }
        public bool Singleton { get; set; }

        public SettingsAttribute(Type settingsType, string @group)
        {
            if (settingsType == null)
                throw new ArgumentNullException("settingsType");
            if (@group == null)
                throw new ArgumentNullException("group");

            SettingsType = settingsType;
            Group = @group;
            Singleton = true;
        }
    }
}
