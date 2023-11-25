using System;

namespace SmartHome.Common.Exceptions;

public class ApplicationMessageException : Exception
{
    public ApplicationMessageException(Exception innerException)
        : base("Unable to parse mqtt message. See inner Exception for more details", innerException)
    {

    }
}
