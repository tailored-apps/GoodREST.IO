using goodREST.Annotations;
using goodREST.Enums;
using goodREST.Interfaces;
using Wise.goodREST.Core.Test.DataModel.DTO;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{

    [Route("Customers/", HttpVerb.POST)]
    public class PostCustomer : IHasResponse<PostCustomerResponse>
    {
        public Customer Customer { get; set; }
        public string CorrelationId { get; set; }
    }
}
