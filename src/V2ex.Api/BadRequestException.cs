using System;

namespace V2ex.Api;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base($"Bad Request: {message}")
    {

    }
}
