using Newtonsoft.Json;
using Wise.goodREST.Core.Interface;

namespace Wise.goodREST.Core.Serializers
{
    public class JsonSerializer : IRequestResponseSerializer
    {
        public string ContentType
        {
            get
            {
                return "application/json";
            }
            
        }

        public string Serialize<T>(T o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public T Deserialize<T>(string o)
        {
            return JsonConvert.DeserializeObject<T>(o);
        }

    }
}
