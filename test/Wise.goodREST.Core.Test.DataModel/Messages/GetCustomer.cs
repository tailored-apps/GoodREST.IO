using Wise.goodREST.Core.Interfaces;
using Wise.goodREST.Core.Annotations;
using Wise.goodREST.Core.Enums;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    [Route("/Customers", HttpVerb.PUT)]
    public class GetCustomer : IHasResponse<GetCustomerResponse>
    {
        public string UserName { get; set; }

        public string TokenId { get; set; }
        public string CorrelationId { get; set; }
    }
}
