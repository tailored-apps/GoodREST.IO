using GoodREST.Extensions.HealthCheck.Messages;
using GoodREST.Middleware.Services;
using System;

namespace GoodREST.Extensions.HealthCheck
{
    internal class HealthCheckService : ServiceBase
    {
        public GetHealthCheckResponse Get(Messages.GetHealthCheck request)
        {
            var response = new GetHealthCheckResponse();
            try
            {
                response.Message = "Alive!";
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