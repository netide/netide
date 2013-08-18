using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Settings
{
    public interface ISettings
    {
        string Group { get; }
        Type SettingsType { get; }
        bool Singleton { get; }

        void Reload();

        event EventHandler Changed;
    }
}
