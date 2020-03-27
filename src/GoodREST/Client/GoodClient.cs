using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoodREST.Client
{
    public abstract class GoodClient<T> where T : IRequestResponseSerializer
    {
        public GoodClient()
        {
        }

        protected string authToken;

        public virtual void SetAuthorizationTokenHeader(string authToken)
        {
            this.authToken = authToken;
        }

        public abstract R Get<R, K>(K request) where K : IHasResponse<R> where R : IResponse;

        public abstract R Post<R, K>(K request) where K : IHasResponse<R> where R : IResponse;

        public abstract R Put<R, K>(K request) where K : IHasResponse<R> where R : IResponse;

        public abstract R Patch<R, K>(K request) where K : IHasResponse<R> where R : IResponse;

        public abstract R Delete<R, K>(K request) where K : IHasResponse<R> where R : IResponse;

        public abstract R Get<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse;

        public abstract R Post<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse;

        public abstract R Put<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse;

        public abstract R Patch<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse;

        public abstract R Delete<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse;

        private  Dictionary<Type, KeyValuePair<string, HttpVerb>> dict = new Dictionary<Type, KeyValuePair<string, HttpVerb>>();

        protected string GetRequestUrl<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            if (!dict.ContainsKey(typeof(K)))
            {
                var attrib = typeof(K).GetTypeInfo().GetCustomAttributes<RouteAttribute>();
                if (attrib.Count() > 1)
                {
                    throw new ArgumentException("Two or more routes, Can't obtain right route, please specify in argument");
                }
                foreach (var item in attrib)
                {
                    dict.Add(typeof(K), new KeyValuePair<string, HttpVerb>(item.Path, (HttpVerb)item.Verb));
                }
            }
            return dict[typeof(K)].Key;
        }

        protected HttpVerb GetRequestVerbs<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            if (!dict.ContainsKey(typeof(K)))
            {
                var attrib = typeof(K).GetTypeInfo().GetCustomAttributes<RouteAttribute>();

                if (attrib.GroupBy(x => x.Verb).Count() > 1)
                {
                    throw new Exception("Can't mix verbs on request");
                }
                var item = attrib.First();

                dict.Add(typeof(K), new KeyValuePair<string, HttpVerb>(item.Path, (HttpVerb)item.Verb));
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

        protected string MapVariablesToRequestValues<R, K>(K request) where K : IHasResponse<R> where R : IResponse
        {
            return MapVariablesToRequestValues<R, K>(request, GetRequestUrl<R, K>(request));
        }

        protected string MapVariablesToRequestValues<R, K>(K request, string url) where K : IHasResponse<R> where R : IResponse
        {
            var urlPattern = url;
            var urlValuesForTokens = GetPropertiesValuesAsStrings(request);
            foreach (var token in urlValuesForTokens)
            {
                urlPattern = urlPattern.Replace(string.Format("{{{0}}}", token.Key), token.Value);
            }
            return urlPattern;
        }
    }
}