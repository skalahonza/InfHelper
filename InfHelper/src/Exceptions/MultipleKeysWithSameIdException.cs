using System;
using System.Runtime.Serialization;

namespace InfHelper.Exceptions
{
    public class MultipleKeysWithSameIdException : InfParserException
    {
        public MultipleKeysWithSameIdException()
        {
        }

        public MultipleKeysWithSameIdException(string message) : base(message)
        {
        }

        public MultipleKeysWithSameIdException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MultipleKeysWithSameIdException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
