using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiSettings : ServiceObject, INiSettings, IServiceProvider
    {
        private readonly INiPackage _package;
        private readonly NiConnectionPoint<INiSettingsNotify> _connectionPoint = new NiConnectionPoint<INiSettingsNotify>();
        private readonly string _registryKey;

        public NiSettings(INiPackage package)
        {
            if (package == null)
                throw new ArgumentNullException("package");

            _package = package;

            var env = (INiEnv)package.GetService(typeof(INiEnv));

            _registryKey =
                env.RegistryRoot +
                "\\Configuration\\Packages\\" +
                package.GetType().GUID.ToString("B").ToUpperInvariant();
        }

        public HResult Advise(object sink, out int cookie)
        {
            return _connectionPoint.Advise(sink, out cookie);
        }

        public HResult Advise(INiSettingsNotify sink, out int cookie)
        {
            return Advise((object)sink, out cookie);
        }

        public HResult Unadvise(int cookie)
        {
            return _connectionPoint.Unadvise(cookie);
        }

        protected RegistryKey OpenRegistryKey(bool writable)
        {
            if (writable)
                return Registry.CurrentUser.CreateSubKey(_registryKey);
            else
                return Registry.CurrentUser.OpenSubKey(_registryKey);
        }

        public HResult GetSetting(string key, out string value)
        {
            value = null;

            try
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                using (var registryKey = OpenRegistryKey(false))
                {
                    if (registryKey != null)
                    {
                        object rawValue = registryKey.GetValue(key);

                        if (rawValue is string)
                            value = (string)rawValue;
                        else if (rawValue is int)
                            value = ((int)rawValue).ToString(CultureInfo.InvariantCulture);

                        if (value != null)
                            return HResult.OK;
                    }

                    return HResult.False;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public HResult SetSetting(string key, string value)
        {
            try
            {
                if (key == null)
                    throw new ArgumentNullException("key");

                using (var registryKey = OpenRegistryKey(true))
                {
                    if (value == null)
                    {
                        registryKey.DeleteValue(key, false);
                    }
                    else
                    {
                        int intValue;

                        if (int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out intValue))
                            registryKey.SetValue(key, intValue);
                        else
                            registryKey.SetValue(key, value);
                    }

                    _connectionPoint.ForAll(p => p.OnSettingChanged(key, value));

                    return HResult.OK;
                }
            }
            catch (Exception ex)
            {
                return ErrorUtil.GetHResult(ex);
            }
        }

        public object GetService(Type serviceType)
        {
            return _package.GetService(serviceType);
        }
    }
}
