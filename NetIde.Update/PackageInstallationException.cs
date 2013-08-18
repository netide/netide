using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetIde.Update
{
    [Serializable]
    public class PackageInstallationException : PackageUpdateException
    {
        public int ExitCode { get; private set; }

        public PackageInstallationException(string message, int exitCode)
            : base(message)
        {
            ExitCode = exitCode;
        }

        public PackageInstallationException(string message, int exitCode, Exception innerException)
            : base(message, innerException)
        {
            ExitCode = exitCode;
        }

        protected PackageInstallationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ExitCode = info.GetInt32("ExitCode");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("ExitCode", ExitCode);
        }
    }
}
