using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Extensions.ServiceDiscovery.Model;
using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Extensions.ServiceDiscovery.DataModel.Messages
{
    [Route("services", HttpVerb.POST)]
    public class PostRegisterInServiceDiscovery : IHasResponse<PostRegisterInServiceDiscoveryResponse>
    {
        public string ServiceHost { get; set; }
        public int Port { get; set; }
        public int ProcessId { get; set; }
        public string Version { get; set; }
        public ICollection<Operation> Operations { get; set; }
        public string AppDomainName { get;  set; }
        public string ApiPrefix { get;  set; }
    }
}