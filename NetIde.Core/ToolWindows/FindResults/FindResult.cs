using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Core.ToolWindows.FindResults
{
    internal class FindResult
    {
        public string FileName { get; private set; }
        public int Offset { get; private set; }
        public int Length { get; private set; }

        public FindResult(string fileName, int offset, int length)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;
            Offset = offset;
            Length = length;
        }
    }
}
