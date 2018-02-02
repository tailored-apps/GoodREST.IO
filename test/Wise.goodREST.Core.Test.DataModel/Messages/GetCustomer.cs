using goodREST.Interfaces;
using goodREST.Annotations;
using goodREST.Enums;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/{UserName}", HttpVerb.GET)]
    public class GetCustomer : IHasResponse<GetCustomerResponse>
    {
        public string UserName { get; set; }

        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}
