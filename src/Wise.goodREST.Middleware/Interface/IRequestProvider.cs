using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Middleware.Interface
{
    public interface IRequestProvider
    {
        void SetRequest(HttpRequest request);
    }
}
