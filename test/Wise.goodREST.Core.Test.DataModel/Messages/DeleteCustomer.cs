using GoodREST.Interfaces;
using GoodREST.Annotations;
using GoodREST.Enums;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/{Id}", HttpVerb.DELETE)]
    public class DeleteCustomer : IHasResponse<DeleteCustomerResponse>
    {
        public int Id { get; set; }

        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}
