using System;

namespace GoodREST.Enums
{
    [Flags]
    public enum HttpVerb
    {
        GET = 1,
        POST = 2,
        PUT = 4,
        DELETE = 8,
        PATCH = 16
    }
}