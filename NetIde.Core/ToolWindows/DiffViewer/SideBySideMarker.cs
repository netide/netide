using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Core.ToolWindows.DiffViewer
{
    internal struct SideBySideMarker
    {
        private readonly SideBySideMarkerType _type;
        private readonly int _line;
        private readonly int _length;

        public SideBySideMarkerType Type
        {
            get { return _type; }
        }

        public int Line
        {
            get { return _line; }
        }

        public int Length
        {
            get { return _length; }
        }

        public SideBySideMarker(SideBySideMarkerType type, int line, int length)
        {
            _type = type;
            _line = line;
            _length = length;
        }
    }
}
