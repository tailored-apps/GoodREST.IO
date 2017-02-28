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
        public IDictionary<string, IDictionary<string, string>> responses { get; set; }
        public IEnumerable<IDictionary<string, IEnumerable<string>>> security { get; set; }
        public void AddResponse(response response)
        {

            if (responses == null) { responses = new Dictionary<string, IDictionary<string, string>>(); }
            if (!responses.ContainsKey(response.code))
            {
                responses.Add(response.code, new Dictionary<string, string>());
            }
            var responseDescription = responses[response.code];
            if (!string.IsNullOrWhiteSpace(response.description.description)) { responseDescription.Add("description", response.description.description); }
        }
    }
}