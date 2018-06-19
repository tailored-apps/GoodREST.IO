using System.Collections.Generic;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class path
    {
        public string value { get; set; }
        public IEnumerable<verb> verbs { get; set; }
    }
}