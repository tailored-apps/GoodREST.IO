using GoodREST.Extensions.ServiceDiscovery.Model;
using GoodREST.Messages;
using System.Collections.Generic;
using System.Linq;

namespace GoodREST.Extensions.ServiceDiscovery.DataModel.Messages
{
    public class GetAllOperationsResponse : ResponseBase
    {
        public ICollection<Operation> Operations { get; set; }
    }
}