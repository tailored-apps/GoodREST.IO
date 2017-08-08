using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wise.goodREST.Core.Annotations;
using Wise.goodREST.Core.Enums;
using Wise.goodREST.Core.Interface;
using Wise.goodREST.Core.Interfaces;

namespace Wise.goodREST.Core.Client
{
    public abstract class GoodClient<T> where T : IRequestResponseSerializer
    {
        private readonly T serializer;
        public GoodClient()
        {
            this.serializer = serializer;
        }

        public abstract R Get<R, K>(K request) where K : IHasResponse<R> where R : IResponse;
        public abstract R Post<R, K>(K request) where K : IHasResponse<R> where R : IResponse;
        public abstract R Put<R, K>(K request) where K : IHasResponse<R> where R : IResponse;
        public abstract R Delete<R, K>(K request) where K : IHasResponse<R> where R : IResponse;


        private static Dictionary<Type, KeyValuePair<string, HttpVerb>> dict = new Dictionary<Type, KeyValuePair<string, HttpVerb>>();
        protected string GetRequestUrl<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            if (!dict.ContainsKey(typeof(K)))
            {
                var attrib = typeof(K).GetTypeInfo().GetCustomAttributes<RouteAttribute>();
                foreach (var item in attrib)
                {
                    dict.Add(typeof(K), new KeyValuePair<string, HttpVerb>(item.Path, (HttpVerb)item.Verb));

                }
            }
            return dict[typeof(K)].Key;
        }
        protected HttpVerb GetRequestVerb<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            if (!dict.ContainsKey(typeof(K)))
            {
                var attrib = typeof(K).GetTypeInfo().GetCustomAttributes<RouteAttribute>();

                foreach (var item in attrib)
                {

                    dict.Add(typeof(K), new KeyValuePair<string, HttpVerb>(item.Path, (HttpVerb)item.Verb));
                }
            }
            return dict[typeof(K)].Value;
        }

        protected Dictionary<string, string> GetPropertiesValuesAsStrings(object request)
        {
            var result = new Dictionary<string, string>();
            var properties = request.GetType().GetTypeInfo().GetProperties();
            foreach (var property in properties)
            {
                var val = property.GetValue(request);
                if (val != null)
                {
                    result.Add(property.Name, val.ToString());
                }
            }
            return result;
        }

    }
}
