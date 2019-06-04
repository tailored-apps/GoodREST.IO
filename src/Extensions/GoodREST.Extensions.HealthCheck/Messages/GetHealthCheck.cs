using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Extensions.HealthCheck.Messages
{
    [MessageDescription(Description = "Message used for checking service health status")]
    [Route("healthcheck", HttpVerb.GET)]
    public class GetHealthCheck : IHasResponse<GetHealthCheckResponse>
    {
    }
}