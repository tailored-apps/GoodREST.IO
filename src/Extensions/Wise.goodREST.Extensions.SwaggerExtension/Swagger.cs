using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class Swagger
    {
        public string swagger { get; set; }
        public info info { get; set; }
        public string host { get; set; }
        public string basePath { get; set; }
        public IEnumerable<tag> tags { get; set; }
        public IEnumerable<string> schemes { get; set; }
        public IDictionary<string,IEnumerable<IDictionary<string, pathDescription>>> paths { get; set; }
        public IEnumerable<securityDefinition> securityDefinition { get; set; }
        public IDictionary<string, IDictionary<string, property>> definitions { get; set; }
        public externalDocs externalDocs { get; set; }
    }
}
