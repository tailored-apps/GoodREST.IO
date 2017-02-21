using System.Collections.Generic;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class pathDescription
    {

        public IEnumerable<string> tags { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string operationId { get; set; }
        public IEnumerable<string> consumes { get; set; }
        public IEnumerable<string> produces { get; set; }
        public IEnumerable<parameter> parameters { get; set; }
        public IEnumerable<response> responses { get; set; }
        public IEnumerable<verbSecurity> security { get; set; }
    }
}