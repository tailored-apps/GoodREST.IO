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
        Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo> services = new Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo>();

        Dictionary<Type, IList<MethodInfo>> serviceMethods = new Dictionary<Type, IList<MethodInfo>>();

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
            serviceMethods.Add(typeof(T), new List<MethodInfo>(methods));


        }

        public void Build()
        {
            foreach (var element in types)
            {
                var returnType = element.Value.GetInterfaces()
                    .Where(x => x.Name == "IHasResponse`1")
                    .Select(x => x.GetGenericArguments())
                    .Single().Single();
                
                var matchingMethod = serviceMethods.Where(X => X.Value.Any(y=>y.ReturnType == returnType && y.GetParameters().SingleOrDefault(z=>z.ParameterType == element.Value) !=null));
                if (matchingMethod != null)
                {
                    //services.Add(
                }
            }
        }
    }
}