using GoodREST.Interfaces;
using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Core.Test.DataModel.DTO;

namespace GoodREST.Core.Test.DataModel.Messages
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
