using Newtonsoft.Json;
using System;
using GoodREST.Interfaces;

namespace GoodREST.Serializers
{
    public class JsonSerializerCharsetUtf8 : IRequestResponseSerializer
    {
        public string ContentType
        {
            get
            {
                return "application/json;charset=UTF-8";
            }
            
        }

        public string Serialize<T>(T o)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
             {
                 Formatting = Newtonsoft.Json.Formatting.Indented,
                 ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                 
             };
            return JsonConvert.SerializeObject(o);
        }

        public T Deserialize<T>(string o)
        {
            return JsonConvert.DeserializeObject<T>(o);
        }
        public object Deserialize(Type type ,string o)
        {
            return JsonConvert.DeserializeObject(o,type);
        }

    }
}
