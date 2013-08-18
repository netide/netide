using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.JobManager
{
    internal class JobEventArgs : EventArgs
    {
        public NiJob Job { get; private set; }

        public JobEventArgs(NiJob job)
        {
            Job = job;
        }
    }

    internal delegate void JobEventHandler(object sender, JobEventArgs e);
}
