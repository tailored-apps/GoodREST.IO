using System;

namespace GoodREST.Extensions.ServiceDiscovery.Model
{
    public class EndpointHosts
    {
        public string ServiceHost { get; set; }
        public int Port { get; set; }
        public int ProcessId { get; set; }
        public DateTime LastHealthCheckUtc { get; set; }

        public override int GetHashCode()
        {
            return $"{this.ServiceHost}:{Port}:{ProcessId}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var oth = obj as EndpointHosts;
            return oth != null && this.GetHashCode() == oth.GetHashCode();
        }

        public static EndpointHosts Create(string serviceHost, int port, int processId)
        {
            return new EndpointHosts
            {
                ServiceHost = serviceHost,
                Port = port,
                ProcessId = processId
            };
        }
    }
}