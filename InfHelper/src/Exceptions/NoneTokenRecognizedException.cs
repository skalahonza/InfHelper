using System;
using System.Runtime.Serialization;

namespace InfHelper.Exceptions
{
    public class NoneTokenRecognizedException : InfParserException
    {
        public NoneTokenRecognizedException()
        {
        }

        public NoneTokenRecognizedException(string message) : base(message)
        {
        }

        public NoneTokenRecognizedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoneTokenRecognizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}