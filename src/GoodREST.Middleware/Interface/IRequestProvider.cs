using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GoodREST.Middleware.Interface
{
    public interface IRequestProvider
    {
        IHeaderDictionary GetHeader();
    }
}
