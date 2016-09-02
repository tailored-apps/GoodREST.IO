using System;
using System.Collections.Generic;
using System.Reflection;
using Wise.goodREST.Core.Annotations;
using System.Linq;
using Wise.goodREST.Core.Enums;

namespace Wise.goodREST.Middleware
{
    public class RestModel : IRestModel
    {
        List<Type> types = new List<Type>();
        public void RegisterMessageModel<T>()
        {
            types.Add(typeof(T));
        }

        public Dictionary<KeyValuePair<string, HttpVerb>, Type> GetRouteForType()
        {
            var dict = new Dictionary<KeyValuePair<string, HttpVerb>, Type>();

            foreach (Type item in types)
            {
                var attrib = item.GetTypeInfo().GetCustomAttributes<RouteAttribute>().SingleOrDefault();
                foreach (Enum en in Enum.GetValues(typeof(HttpVerb)))
                {
                    if (attrib.Verb.HasFlag(en))
                    {
                        dict.Add(new KeyValuePair<string, HttpVerb>(attrib.Path, (HttpVerb)en), item);
                    }
                }


            }

            return dict;
        }
    }
}