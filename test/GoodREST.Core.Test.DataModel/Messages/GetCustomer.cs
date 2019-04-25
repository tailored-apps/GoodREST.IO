using GoodREST.Interfaces;
using GoodREST.Annotations;
using GoodREST.Enums;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers", HttpVerb.GET)]
    public class GetCustomers : IHasResponse<GetCustomersResponse>
    {
        
    }
}
