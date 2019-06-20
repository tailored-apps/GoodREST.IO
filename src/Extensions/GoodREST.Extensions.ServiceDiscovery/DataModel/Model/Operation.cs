using System;

namespace GoodREST.Extensions.ServiceDiscovery.Model
{
    public class Operation
    {
        public string Path { get; set; }
        public string Verb { get; set; }
        public string Version { get; set; }
    }
}