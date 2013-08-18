using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    [Serializable]
    public class NiLogEvent : INiLogEvent
    {
        public DateTime TimeStamp { get; set; }
        public string Content { get; set; }
        public string Message { get; set; }
        public int Severity { get; set; }
        public string Source { get; set; }
    }
}
