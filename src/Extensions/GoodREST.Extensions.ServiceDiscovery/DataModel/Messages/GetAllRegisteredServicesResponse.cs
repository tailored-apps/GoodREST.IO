using System.Collections.Generic;
using GoodREST.Messages;

namespace GoodREST.Extensions.ServiceDiscovery.DataModel.Messages
{
    public class GetAllRegisteredServicesResponse : ResponseBase
    {
        public ICollection<Model.Service> Services { get; set; }
    }
}