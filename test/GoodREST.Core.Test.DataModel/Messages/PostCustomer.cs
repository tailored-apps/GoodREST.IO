using GoodREST.Annotations;
using GoodREST.Core.Test.DataModel.DTO;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    [Route("Customers/", HttpVerb.POST)]
    public class PostCustomer : IHasResponse<PostCustomerResponse>
    {
        public Customer Customer { get; set; }
        public string CorrelationId { get; set; }
    }
}