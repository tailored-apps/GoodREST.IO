using System;
using System.Text;
using GoodREST.Extensions.HealthCheck.Messages;
using GoodREST.Middleware.Interface;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace GoodREST.Extensions.HealthCheck
{
    public class HealthCheckService : ServiceBase
    {
        public CheckResponse Get(Check request)
        {
            var response = new CheckResponse();
            try
            {
                response.Message = "Alive";
                response.Ok();

            }
            catch (Exception e)
            {
                response.ConvertExceptionAsError(e);
            }

            return response;
        }
    }
}
