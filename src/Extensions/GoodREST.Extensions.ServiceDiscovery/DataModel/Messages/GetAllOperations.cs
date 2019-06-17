using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Extensions.ServiceDiscovery.DataModel.Messages
{
    [Route("operations", HttpVerb.GET)]
    public class GetAllOperations : IHasResponse<GetAllOperationsResponse>
    {
    }
}