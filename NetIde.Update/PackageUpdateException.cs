using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetIde.Update
{
    [Serializable]
    public class PackageUpdateException : Exception
    {
        public PackageUpdateException()
        {
        }

        public PackageUpdateException(string message)
            : base(message)
        {
        }

        public PackageUpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PackageUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
