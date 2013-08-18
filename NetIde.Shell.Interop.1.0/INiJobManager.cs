using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Shell.Interop
{
    public interface INiJobManager
    {
        HResult CreateJob(INiJobHandler handler, out INiJob job);
        HResult Enqueue(params INiJob[] jobs);
        HResult WaitForJob(INiJob job);
        HResult WaitForAll();
        HResult WaitForAll(bool showDialog);
    }
}
