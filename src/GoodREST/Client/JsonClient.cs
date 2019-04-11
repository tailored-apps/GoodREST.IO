using System;
using GoodREST.Enums;
using Newtonsoft.Json;
using GoodREST.Interfaces;
using System.Net.Http;
using System.Linq;

namespace GoodREST.Client
{
    public class JsonClient : GoodClient<Serializers.JsonSerializer>, IDisposable
    {
        HttpClient client;
        JsonSerializer serializer;
        public JsonClient(string endpointAddress)
        {
            serializer = new Newtonsoft.Json.JsonSerializer();
            client = new System.Net.Http.HttpClient();
            Uri uri;
            if (Uri.TryCreate(endpointAddress, UriKind.Absolute, out uri))
            {
                client.BaseAddress = uri;
            }

        }

        public override void SetAuthorizationTokenHeader(string authToken)
        {
            base.SetAuthorizationTokenHeader(authToken);
            if (!string.IsNullOrEmpty(authToken) )
            {
                client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
            }
        }

        public override R Delete<R, K>(K request)
        {
            var verb = GetRequestVerbs<R, K>(request);
            if (verb != HttpVerb.DELETE)
            {
                throw new InvalidOperationException("Http verb different than DELETE is not allowed for that operation");
            }

            var call = client.DeleteAsync(MapVariablesToRequestValues<R, K>(request)).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public void Dispose()
        {
            client.Dispose();
        }

        public override R Get<R, K>(K request)
        {
            var verb = GetRequestVerbs<R, K>(request);
            if (verb != HttpVerb.GET)
            {
                throw new InvalidOperationException("Http verb different than GET is not allowed for that operation");
            }
            var call = client.GetAsync(MapVariablesToRequestValues<R, K>(request)).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Post<R, K>(K request)
        {
            var textWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, request);
            

            var data = new StringContent(textWriter.ToString());
            var url = MapVariablesToRequestValues<R, K>(request);
            var call = client.PostAsync(url, data).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Patch<R, K>(K request)
        {
            var textWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, request);

            var data = new StringContent(textWriter.ToString());
            var url = MapVariablesToRequestValues<R, K>(request);
            var call = client.SendAsync(new HttpRequestMessage() { RequestUri = new Uri(client.BaseAddress, url), Content = data, Method = HttpMethod.Put }).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Put<R, K>(K request)
        {
            var textWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, request);

            var data = new StringContent(textWriter.ToString());
            client.DefaultRequestHeaders.Authorization = System.Net.Http.Headers.AuthenticationHeaderValue.Parse("x");
            var call = client.PutAsync(MapVariablesToRequestValues<R, K>(request), data).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }


        public override R Get<R, K>(K request, string url)
        {
            var verb = GetRequestVerbs<R, K>(request);
            if (verb != HttpVerb.GET)
            {
                throw new InvalidOperationException("Http verb different than GET is not allowed for that operation");
            }
            var call = client.GetAsync(MapVariablesToRequestValues<R, K>(request,url)).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Post<R, K>(K request, string url)
        {

            var textWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, request);

            var data = new StringContent(textWriter.ToString());
            
            var call = client.PostAsync(MapVariablesToRequestValues<R, K>(request, url), data).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);


        }

        public override R Put<R, K>(K request, string url)
        {

            var textWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(textWriter);

            serializer.Serialize(jsonWriter, request);

            var data = new StringContent(textWriter.ToString());
            var call = client.PutAsync(MapVariablesToRequestValues<R, K>(request,url), data).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Patch<R, K>(K request, string url)
        {
            throw new NotImplementedException();
        }

        public override R Delete<R, K>(K request, string url)
        {

            var verb = GetRequestVerbs<R, K>(request);
            if (verb != HttpVerb.DELETE)
            {
                throw new InvalidOperationException("Http verb different than DELETE is not allowed for that operation");
            }

            var call = client.DeleteAsync(MapVariablesToRequestValues<R, K>(request)).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StreamReader(result.ReadAsStreamAsync().GetAwaiter().GetResult());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);
        }
    }
}
