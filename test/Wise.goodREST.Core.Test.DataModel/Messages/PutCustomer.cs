using Wise.goodREST.Core.Interfaces;
using Wise.goodREST.Core.Annotations;
using Wise.goodREST.Core.Enums;
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
