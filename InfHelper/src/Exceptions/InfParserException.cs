using System;
using System.Runtime.Serialization;

namespace InfHelper.Exceptions
{
    public class InfParserException : Exception
    {
        public InfParserException()
        {
        }

        public InfParserException(string message) : base(message)
        {
        }

        public InfParserException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InfParserException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
