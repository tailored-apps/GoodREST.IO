using GoodREST.Interfaces;
using GoodREST.Annotations;
using GoodREST.Enums;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/{UserName}", HttpVerb.GET)]
    public class GetCustomer : IHasResponse<GetCustomerResponse>
    {
        public string UserName { get; set; }

        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}
