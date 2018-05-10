using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodREST.Middleware.Interface;

namespace GoodREST.Middleware.Services
{
    public abstract class ServiceBase
    {
        public IAuthService AuthService { get; set; }
        public ISecurityService SecurityService { get; set; }
    }
}
