using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Setup.Installation
{
    public interface IProgress
    {
        void AddLog(string message);

        void SetProgress(double progress);
    }
}
