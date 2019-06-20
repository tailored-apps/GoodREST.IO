using System.Collections.Generic;
using System.Linq;
using GoodREST.Extensions.ServiceDiscovery.Model;

namespace GoodREST.Extensions.ServiceDiscovery.Middleware.Services
{
    public interface IServiceInfoPersister
    {
        ICollection<Service> GetServices();

        ICollection<Operation> GetOperations();

        void Register(string appDomainName, string version, string apiPrefix, ICollection<Operation> operations, EndpointHosts endpoint);
    }
}