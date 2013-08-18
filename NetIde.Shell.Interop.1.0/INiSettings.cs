using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiSettings : INiConnectionPoint
    {
        HResult Advise(INiSettingsNotify sink, out int cookie);
        HResult GetSetting(string key, out string value);
        HResult SetSetting(string key, string value);
    }
}
