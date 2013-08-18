using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal interface IViewer
    {
        void LoadStreams(IStream leftStream, FileType leftFileType, byte[] leftData, IStream rightStream, FileType rightFileType, byte[] rightData);
    }
}
