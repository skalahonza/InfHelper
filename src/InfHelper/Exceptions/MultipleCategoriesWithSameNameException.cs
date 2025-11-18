using System;
using System.Runtime.Serialization;

namespace InfHelper.Exceptions;

public class MultipleCategoriesWithSameNameException : InfParserException
{
    public MultipleCategoriesWithSameNameException()
    {
    }

    public MultipleCategoriesWithSameNameException(string message) : base(message)
    {
    }

    public MultipleCategoriesWithSameNameException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
