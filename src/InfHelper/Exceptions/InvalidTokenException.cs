using System;

namespace InfHelper.Exceptions;

public class InvalidTokenException : InfParserException
{
    public InvalidTokenException()
    {
    }

    public InvalidTokenException(string message) : base(message)
    {
    }

    public InvalidTokenException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
