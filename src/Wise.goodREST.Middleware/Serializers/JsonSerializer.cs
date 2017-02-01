using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wise.goodREST.Middleware.Interface;

namespace Wise.goodREST.Middleware.Serializers
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
