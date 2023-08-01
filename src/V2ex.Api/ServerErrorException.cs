using System;

namespace V2ex.Api;

public class ServerErrorException : Exception
{
    public ServerErrorException(string message) : base($"Server Error: {message}")
    {

    }
}
