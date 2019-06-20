using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Extensions.ServiceDiscovery.Config
{
    public class ServiceConfiguration
    {
        public Endpoint Endpoint { get; set; }
        public Endpoint SecureEndpoint { get; set; }
        public Api Api { get; set; }
        public int SecurePort { get;  set; }
        public bool FromEnvironmentVariable { get; set; }
        public string ServiceDiscoveryUrl { get;  set; }
    }
}