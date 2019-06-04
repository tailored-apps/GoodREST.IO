using System.Collections.Generic;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class parameter
    {
        public string @in { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool required { get; set; }
        public IDictionary<string, string> schema { get; set; }
    }
}