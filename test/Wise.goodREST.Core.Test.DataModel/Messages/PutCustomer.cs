using goodREST.Interfaces;
using goodREST.Annotations;
using goodREST.Enums;
using Wise.goodREST.Core.Test.DataModel.DTO;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/{Id}", HttpVerb.PUT)]
    public class PutCustomer : IHasResponse<PutCustomerResponse>
    {
        public Customer Customer { get; set; }
        public string Id { get; set; }

        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}
