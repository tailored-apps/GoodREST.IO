using Wise.goodREST.Core.Annotations;
using Wise.goodREST.Core.Enums;
using Wise.goodREST.Core.Interfaces;
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
