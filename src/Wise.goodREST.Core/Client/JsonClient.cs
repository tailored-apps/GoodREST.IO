using System;
using Wise.goodREST.Core.Enums;
using Newtonsoft.Json;
using Wise.goodREST.Core.Interfaces;
namespace Wise.goodREST.Core.Client
{
    public class JsonClient : GoodClient<Serializers.JsonSerializer>, IDisposable
    {
        System.Net.Http.HttpClient client;
        Newtonsoft.Json.JsonSerializer serializer;
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

        public override R Delete<R, K>(K request)
        {
            var verb = GetRequestVerb<R, K>(request);
            if (verb != HttpVerb.DELETE)
            {
                throw new InvalidOperationException("Http verb different than DELETE is not allowed for that operation");
            }
            return serializer.Deserialize<R>(new JsonTextReader(new System.IO.StringReader(client.DeleteAsync(MapVariablesToRequestValues<R, K>(request)).GetAwaiter().GetResult().Content.ToString())));
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public override R Get<R, K>(K request)
        {
            var verb = GetRequestVerb<R, K>(request);
            if (verb != HttpVerb.GET)
            {
                throw new InvalidOperationException("Http verb different than GET is not allowed for that operation");
            }
            var call = client.GetAsync(MapVariablesToRequestValues<R, K>(request)).GetAwaiter();
            var result = call.GetResult().Content;
            var reader = new System.IO.StringReader(result.ToString());
            var jsonReader = new JsonTextReader(reader);
            return serializer.Deserialize<R>(jsonReader);

        }

        public override R Post<R, K>(K request)
        {
            throw new NotImplementedException();
        }

        public override R Put<R, K>(K request)
        {
            throw new NotImplementedException();

        }

        private string MapVariablesToRequestValues<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            var urlPattern = GetRequestUrl<R, K>(request);
            var urlValuesForTokens = GetPropertiesValuesAsStrings(request);
            foreach (var token in urlValuesForTokens)
            {
                urlPattern = urlPattern.Replace(string.Format("{{{0}}}", token.Key), token.Value);
            }
            return urlPattern;
        }

    }
}
