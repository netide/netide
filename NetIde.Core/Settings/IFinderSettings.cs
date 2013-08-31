using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;

namespace NetIde.Core.Settings
{
    [Settings(typeof(INiSettings), "Finder")]
    public interface IFinderSettings
    {
        [DefaultValue(NiFindOptions.Find | NiFindOptions.Project)]
        NiFindOptions Options { get; set; }

        string FindWhatHistory { get; set; }

        string ReplaceWithHistory { get; set; }

        string LookIn { get; set; }

        string LookInHistory { get; set; }

        string LookAtFileTypesHistory { get; set; }
    }
}
