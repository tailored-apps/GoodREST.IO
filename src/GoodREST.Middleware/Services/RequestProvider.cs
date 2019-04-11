using GoodREST.Middleware.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Middleware.Services
{
    internal class RequestProvider : IRequestProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public RequestProvider(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public IHeaderDictionary GetHeader()
        {
            return httpContextAccessor.HttpContext.Request.Headers;
        }
    }
}
