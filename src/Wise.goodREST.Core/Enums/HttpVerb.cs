using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wise.goodREST.Core.Enums
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
