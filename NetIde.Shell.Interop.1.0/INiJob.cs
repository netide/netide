using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiJob
    {
        INiJobHandler Handler { get; }
        string CurrentStatus { get; set; }
        double? Progress { get; set; }
        bool Cancelled { get; set; }
        bool Success { get; }
        bool Completed { get; }
        bool Running { get; }
    }
}
