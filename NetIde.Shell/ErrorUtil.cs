using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NetIde.Shell.Interop;
using log4net;

namespace NetIde.Shell
{
    public static class ErrorUtil
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ErrorUtil));

        [DebuggerStepThrough]
        public static HResult GetHResult(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            Log.Warn("Exception", exception);

            return (HResult)Marshal.GetHRForException(exception);
        }

        [DebuggerStepThrough]
        public static bool ThrowOnFailure(HResult hr)
        {
            Marshal.ThrowExceptionForHR((int)hr);
            return hr != HResult.False;
        }

        [DebuggerStepThrough]
        public static bool Success(HResult hr)
        {
            return hr >= 0;
        }

        [DebuggerStepThrough]
        public static bool Failure(HResult hr)
        {
            return hr < 0;
        }
    }
}
