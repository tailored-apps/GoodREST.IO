using GoodREST.Annotations;
using GoodREST.Core.Test.DataModel.DTO;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/{Id}", HttpVerb.PUT)]
    public class PutCustomer : IHasResponse<PutCustomerResponse>
    {
        public Customer Customer { get; set; }
        public int Id { get; set; }
        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}