using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;

namespace NetIde.Core.Settings
{
    [Settings(typeof(INiSettings), "Diff Viewer")]
    public interface IDiffViewerSettings
    {
        [DefaultValue(NiDiffViewerMode.SideBySide)]
        NiDiffViewerMode DefaultMode { get; set; }
    }
}
