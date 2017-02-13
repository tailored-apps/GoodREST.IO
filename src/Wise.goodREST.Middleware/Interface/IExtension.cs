using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wise.goodREST.Middleware.Interface
{
    public interface IExtension
    {
        Task Swagger(HttpContext builder);
       
    }
}
