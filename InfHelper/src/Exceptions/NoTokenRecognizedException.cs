using System;
using System.Runtime.Serialization;

namespace InfHelper.Exceptions
{
    public class NoTokenRecognizedException : InfParserException
    {
        public NoTokenRecognizedException()
        {
        }

        public NoTokenRecognizedException(string message) : base(message)
        {
        }

        public NoTokenRecognizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoTokenRecognizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}