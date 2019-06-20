using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Extensions.ServiceDiscovery.DataModel.Messages
{
    [Route("services", HttpVerb.GET)]
    public class GetAllRegisteredServices : IHasResponse<GetAllRegisteredServicesResponse>
    {
    }
}