using System;
using System.Collections.Generic;

namespace GoodREST.Extensions.ServiceDiscovery.Model
{
    public class Service
    {
        public ICollection<EndpointHosts> EndpointHosts { get; set; }
        public string AppDomainName { get; set; }
        public string ApiPrefix { get; set; }
        public string Version { get; set; }

        public override int GetHashCode()
        {
            return $"{this.Version}:{ApiPrefix}:{AppDomainName}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var oth = obj as Service;
            return oth != null && this.GetHashCode() == oth.GetHashCode();
        }

        public static Service Create(string appDomainName, string apiPrefix, string version)
        {
            return new Service
            {
                ApiPrefix = apiPrefix,
                AppDomainName = appDomainName,
                Version = version
            };
        }
    }
}