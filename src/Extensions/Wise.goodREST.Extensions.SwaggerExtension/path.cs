using System.Collections.Generic;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class path
    {
        public string value { get; set; }
        public IEnumerable<verb> verbs { get; set; }
    }
}