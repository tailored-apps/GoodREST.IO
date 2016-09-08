using System;
using System.Collections.Generic;
using System.Reflection;
using Wise.goodREST.Core.Annotations;
using System.Linq;
using Wise.goodREST.Core.Enums;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Middleware
{
    public class RestModel : IRestModel
    {
        Dictionary<KeyValuePair<string, HttpVerb>, Type> types = new Dictionary<KeyValuePair<string, HttpVerb>, Type>();
        Dictionary<KeyValuePair<HttpVerb, Type>, Type> services = new Dictionary<KeyValuePair<HttpVerb, Type>, Type>();
        

        public void RegisterMessageModel<T>()
        {
            var attrib = typeof(T).GetTypeInfo().GetCustomAttributes<RouteAttribute>().SingleOrDefault();
            if (attrib != null)
            {
                foreach (Enum en in Enum.GetValues(typeof(HttpVerb)))
                {
                    if (attrib.Verb.HasFlag(en))
                    {
                        types.Add(new KeyValuePair<string, HttpVerb>(attrib.Path, (HttpVerb)en), typeof(T));
                    }
                }
            }

        }

        public Dictionary<KeyValuePair<string, HttpVerb>, Type> GetRouteForType()
        {
            return types;
        }

        public void RegisterSercice<T>() where T : ServiceBase
        {
            var methods = typeof(T).GetTypeInfo().GetMethods(BindingFlags.Public);
            

        }
    }
}