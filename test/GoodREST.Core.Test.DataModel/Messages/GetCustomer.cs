using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Role("TestRole")]
    [Role("TestRole", "myRole")]
    [Route("Customers", HttpVerb.GET)]
    public class GetCustomers : IHasResponse<GetCustomersResponse>
    {
    }
}