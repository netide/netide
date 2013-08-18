using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Shell.Settings;

namespace NetIde.Core.Settings
{
    [Settings(typeof(INiSettings), "Font")]
    public interface IFontSettings
    {
        Font CodeFont { get; set; }
    }
}
