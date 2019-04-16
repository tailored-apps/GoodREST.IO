using System.Collections.Generic;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class response
    {
        public string code { get; set; }
        public Dictionary<string,object> description { get; set; }
    }
}