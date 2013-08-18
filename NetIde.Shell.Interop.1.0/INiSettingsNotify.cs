using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiSettingsNotify
    {
        void OnSettingChanged(string key, string value);
    }
}
