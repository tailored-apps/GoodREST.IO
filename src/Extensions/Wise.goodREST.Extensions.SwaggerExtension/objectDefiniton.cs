using System.Collections.Generic;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class objectDefiniton
    {
        public string type { get; set; }
        public IEnumerable<property> properties { get; set; }

    }
}