using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    [Serializable]
    public struct NiTextSpan
    {
        private readonly int _startLine;
        private readonly int _startIndex;
        private readonly int _endLine;
        private readonly int _endIndex;

        public int StartLine
        {
            get { return _startLine; }
        }

        public int StartIndex
        {
            get { return _startIndex; }
        }

        public int EndLine
        {
            get { return _endLine; }
        }

        public int EndIndex
        {
            get { return _endIndex; }
        }

        public NiTextSpan(int startLine, int startIndex, int endLine, int endIndex)
        {
            _startLine = startLine;
            _startIndex = startIndex;
            _endLine = endLine;
            _endIndex = endIndex;
        }
    }
}
