using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideCommandLineSwitchAttribute : RegistrationAttribute
    {
        public string Name { get; private set; }
        public bool ExpectArgument { get; private set; }

        public ProvideCommandLineSwitchAttribute(string name, bool expectArgument)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            ExpectArgument = expectArgument;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            using (var key = packageKey.CreateSubKey("CommandLine\\" + Name))
            {
                key.SetValue("ExpectArgument", ExpectArgument ? 1 : 0);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
