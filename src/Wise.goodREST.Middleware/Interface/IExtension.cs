using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace Wise.goodREST.Middleware.Interface
{
    public interface IExtension
    {
        void Install(RouteBuilder routeBuilder);
    }
}
