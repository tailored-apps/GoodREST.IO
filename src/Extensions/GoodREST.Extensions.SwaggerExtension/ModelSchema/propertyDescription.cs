using System.Collections.Generic;
using System.Linq;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class propertyDescription
    {
        public propertyDescription()
        {
            @enum = Enumerable.Empty<string>();
        }
        public string type { get; set; }
        public string format { get; set; }
        public string description { get; set; }
        public IEnumerable<string> @enum { get; set; }
    }
}