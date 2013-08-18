using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NetIde.Shell;

namespace NetIde
{
    [Serializable]
    internal class NetIdeException : Exception
    {
        public NetIdeException()
        {
        }

        public NetIdeException(string message)
            : base(message)
        {
        }

        public NetIdeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NetIdeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
