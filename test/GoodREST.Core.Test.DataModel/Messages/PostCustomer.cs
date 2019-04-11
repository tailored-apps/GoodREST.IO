using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;
using GoodREST.Core.Test.DataModel.DTO;

namespace GoodREST.Core.Test.DataModel.Messages
{

    [Route("Customers/", HttpVerb.POST)]
    public class PostCustomer : IHasResponse<PostCustomerResponse>
    {
        public Customer Customer { get; set; }
        public string CorrelationId { get; set; }
    }
}
