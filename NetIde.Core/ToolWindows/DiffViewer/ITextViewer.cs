using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NGit.Diff;
using NetIde.Shell.Interop;
using NetIde.Util;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal interface ITextViewer
    {
        void SelectDetails(IStream leftStream, FileType leftFileType, IStream rightStream, FileType rightFileType);
        void LoadDiff(Text leftText, Text rightText, EditList editList);
    }
}
