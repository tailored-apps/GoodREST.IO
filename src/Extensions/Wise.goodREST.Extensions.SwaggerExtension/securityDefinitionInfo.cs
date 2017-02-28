using System.Collections.Generic;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class securityDefinitionInfo
    {
        public string name;

        public string type { get; set; }
        public string authorizationUrl { get; set; }
        public string @in { get; set; }
        public string flow { get; set; }
        public IDictionary<string,string> scopes { get; set; }
    }
}