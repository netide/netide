using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace NetIde.Xml
{
    [Serializable]
    public class XmlValidationException : Exception
    {
        public XmlValidationException()
        {
        }

        public XmlValidationException(string message)
            : base(message)
        {
        }

        public XmlValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected XmlValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
