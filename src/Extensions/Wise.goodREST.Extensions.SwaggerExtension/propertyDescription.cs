using System.Collections.Generic;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class propertyDescription
    {
        public string type { get; set; }
        public string format { get; set; }
        public string description { get; set; }
        public IEnumerable<string> @enum { get; set; }
    }
}