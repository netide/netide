using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetIde.Services.PackageManager
{
    internal class ExceptionEventArgs
    {
        public Exception Exception { get; private set; }

        public ExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }

    internal delegate void ExceptionEventHandler(object sender, ExceptionEventArgs e);
}
