using System.Text;
using System.Collections.Generic;
using System;

namespace NetIde.Shell.Interop
{
    public interface INiRegistrationKey : IDisposable
    {
        INiRegistrationKey CreateSubKey(string name);
        void SetValue(string valueName, object value);
    }
}
