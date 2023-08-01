using System;

namespace V2ex.Api;

public class NotAuthorizedException : Exception
{
    public NotAuthorizedException() : base("Not Authorized")
    {

    }
}
