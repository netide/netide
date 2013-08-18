using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Shell.Interop;

namespace NetIde.Update
{
    internal class NiRegistrationKey : MarshalByRefObject, INiRegistrationKey
    {
        private RegistryKey _key;
        private bool _disposed;

        public NiRegistrationKey(RegistryKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            _key = key;
        }

        public INiRegistrationKey CreateSubKey(string name)
        {
            return new NiRegistrationKey(_key.CreateSubKey(name));
        }

        public void SetValue(string valueName, object value)
        {
            _key.SetValue(valueName, value);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (_key != null)
                {
                    _key.Dispose();
                    _key = null;
                }

                _disposed = true;
            }
        }
    }
}
