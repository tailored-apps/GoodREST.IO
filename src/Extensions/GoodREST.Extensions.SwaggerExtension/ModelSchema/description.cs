using System.Collections.Generic;
using System.Linq;

namespace GoodREST.Extensions.SwaggerExtension
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
        public IDictionary<string, IDictionary<string, object>> responses { get; set; }
        public IEnumerable<IDictionary<string, IEnumerable<string>>> security { get; set; }

        public void AddResponse(response response)
        {
            if (responses == null) { responses = new Dictionary<string, IDictionary<string, object>>(); }
            if (!responses.ContainsKey(response.code))
            {
                responses.Add(response.code, new Dictionary<string, object>());
            }

            responses[response.code] = response.description;
        }

        public void AddParameter(parameter parameter)
        {
            if (parameters == null) { parameters = new List<parameter>(); }
            var @params = parameters as List<parameter>;
            @params.Add(parameter);
        }

        public void AddSecurity(verbSecurity securityToAdd)
        {
            if (security == null) { security = new List<IDictionary<string, IEnumerable<string>>>(); }
            if (!security.Any(x => x.ContainsKey(securityToAdd.value)))
            {
                var @definitions = security as List<IDictionary<string, IEnumerable<string>>>;
                @definitions.Add(new Dictionary<string, IEnumerable<string>>());
            }
            var responseDescription = security.Single();
            responseDescription.Add(securityToAdd.value, securityToAdd.operations);
        }
    }
}