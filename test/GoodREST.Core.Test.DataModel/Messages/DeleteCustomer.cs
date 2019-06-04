using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

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