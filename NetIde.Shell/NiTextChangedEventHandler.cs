using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell
{
    public class NiTextChangedEventArgs : EventArgs
    {
        public int StartLine { get; private set; }
        public int StartIndex { get; private set; }
        public int EndLine { get; private set; }
        public int EndIndex { get; private set; }

        public NiTextChangedEventArgs(int startLine, int startIndex, int endLine, int endIndex)
        {
            StartLine = startLine;
            StartIndex = startIndex;
            EndLine = endLine;
            EndIndex = endIndex;
        }
    }

    public delegate void NiTextChangedEventHandler(object sender, NiTextChangedEventArgs e);
}
