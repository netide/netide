﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetIde.Shell.Interop;

namespace NetIde.Project.Interop
{
    public interface INiProjectItem : INiHierarchy
    {
        HResult GetFileName(out string fileName);
        HResult SetFileName(string fileName);
        HResult Open(out INiWindowFrame windowFrame);
        HResult Remove();
        HResult Save(string fileName);
        HResult SaveAs(string fileName);
    }
}
