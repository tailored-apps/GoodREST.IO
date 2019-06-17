using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using GoodREST.Extensions.ServiceDiscovery.Model;

namespace GoodREST.Extensions.ServiceDiscovery.Middleware.Services
{
    public class InMemoryServiceInfoPersister : IServiceInfoPersister
    {
        private ConcurrentDictionary<Service, ICollection<Operation>> services = new ConcurrentDictionary<Service, ICollection<Operation>>();

        public ICollection<Service> GetServices()
        {
            return services.Keys;
        }

        public ICollection<Operation> GetOperations()
        {
            return services.SelectMany(x => x.Value.Select(y => y)).ToList();
        }

        public void Register(string appDomainName, string version, string apiPrefix, ICollection<Operation> operations, EndpointHosts endpoint)
        {
            var service = Service.Create(appDomainName, apiPrefix, version);
            if (!services.ContainsKey(service))
            {
                service.EndpointHosts = new List<EndpointHosts>() { endpoint };

                foreach (var operation in operations)
                {
                    var apiVersion = !string.IsNullOrWhiteSpace(operation.Version)
                        ? operation.Version
                        : version;

                    operation.Path = !string.IsNullOrWhiteSpace(apiPrefix) ?
                        $"{apiPrefix}/{apiVersion}/{operation.Path}"
                        : $"{apiVersion}/{operation.Path}";
                }
                while (!services.TryAdd(service, operations))
                {
                    System.Threading.Thread.Sleep(10);
                };
            }
            else
            {
                var endpoints = services.Single(x => x.Key == service).Key.EndpointHosts;
                if (!endpoints.Contains(endpoint))
                {
                    endpoints.Add(endpoint);
                }
            }
        }
    }
}