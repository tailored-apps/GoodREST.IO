using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wise.goodREST.Middleware.Interface;

namespace Wise.goodREST.Middleware.Services
{
    public abstract class ServiceBase
    {
        public HttpContext  Context { get; private set; }
    }
}
