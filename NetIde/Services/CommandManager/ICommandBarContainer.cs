using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Services.CommandManager
{
    internal interface ICommandBarContainer
    {
        INiList<INiCommandBarGroup> Controls { get; }
        event NotifyCollectionChangedEventHandler ItemsChanged;
    }
}
