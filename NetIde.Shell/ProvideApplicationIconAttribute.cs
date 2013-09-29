using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public sealed class ProvideApplicationIconAttribute : RegistrationAttribute
    {
        public string FileName { get; private set; }

        public ProvideApplicationIconAttribute(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;
        }

        public override void Register(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
            using (var key = packageKey.CreateSubKey("ApplicationIcon"))
            {
                key.SetValue(null, FileName);
            }
        }

        public override void Unregister(INiPackage package, INiRegistrationContext context, INiRegistrationKey packageKey)
        {
        }
    }
}
