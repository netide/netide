using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetIde.Test.Support
{
    [Serializable]
    internal class ExitTimeoutException : Exception
    {
        public ExitTimeoutException()
        {
        }

        public ExitTimeoutException(string message)
            : base(message)
        {
        }

        public ExitTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ExitTimeoutException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
