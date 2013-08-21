using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Shell
{
    public class NiProjectItem : NiHierarchy, INiProjectItem
    {
        private string _fileName;

        public HResult GetFileName(out string fileName)
        {
            fileName = _fileName;
            return HResult.OK;
        }

        public HResult SetFileName(string fileName)
        {
            _fileName = fileName;
            return HResult.OK;
        }
    }
}
