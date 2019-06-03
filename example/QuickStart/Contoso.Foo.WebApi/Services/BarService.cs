using System;
using Contoso.Foo.WebApi.Messages;
using GoodREST.Extensions;
using GoodREST.Middleware.Services;

namespace Contoso.Foo.WebApi.Services
{
    public class BarService : ServiceBase
    {
        public GetBarResponse Get(GetBar request)
        {
            var response = new GetBarResponse();
            try
            {
                response.FooBar = "It Works!";
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }
    }
}