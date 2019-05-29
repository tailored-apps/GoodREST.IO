using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers", HttpVerb.GET)]
    public class GetCustomers : IHasResponse<GetCustomersResponse>
    {
    }
}