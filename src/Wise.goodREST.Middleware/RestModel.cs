using System;
using System.Collections.Generic;
using System.Reflection;
using GoodREST.Annotations;
using System.Linq;
using GoodREST.Enums;

namespace GoodREST.Middleware
{
    public class RestModel : IRestModel
    {
        Dictionary<KeyValuePair<string, HttpVerb>, Type> types = new Dictionary<KeyValuePair<string, HttpVerb>, Type>();
        Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo> services = new Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo>();

        Dictionary<Type, IList<MethodInfo>> serviceMethods = new Dictionary<Type, IList<MethodInfo>>();

        public bool IsSecurityEnabled { get; private set; }
        public List<string> SecuirtyExcludedPaths { get; private set; }
        public bool IsSecuritySetToReadOnlyForUnkownAuth { get; private set; }

        public void RegisterMessageModel<T>()
        {
            var attrib = typeof(T).GetTypeInfo().GetCustomAttributes<RouteAttribute>();

            if (attrib != null && attrib.Any())
            {
                foreach (var item in attrib)
                {
                    foreach (Enum en in Enum.GetValues(typeof(HttpVerb)))
                    {
                        if (item.Verb.HasFlag(en))
                        {
                            types.Add(new KeyValuePair<string, HttpVerb>(item.Path, (HttpVerb)en), typeof(T));
                        }
                    }
                }
            }

        }

        public Dictionary<KeyValuePair<string, HttpVerb>, Type> GetRouteForType()
        {
            return types;
        }


        public MethodInfo GetServiceMethodForType(HttpVerb verb, Type requestType)
        {

            return services[new KeyValuePair<HttpVerb, Type>(verb, requestType)];
        }

        public void Build(IEnumerable<Type> registeredServices)
        {
            foreach (var restService in registeredServices)
            {

                var methods = restService.GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                serviceMethods.Add(restService, new List<MethodInfo>(methods));
            }
            foreach (var element in types)
            {
                var returnType = element.Value.GetInterfaces()
                    .Where(x => x.Name == "IHasResponse`1")
                    .Select(x => x.GetGenericArguments())
                    .Single().Single();

                var matchingMethod = serviceMethods.Where(X => X.Value.Any(y => y.ReturnType == returnType && y.GetParameters().SingleOrDefault(z => z.ParameterType == element.Value) != null));
                if (matchingMethod != null && matchingMethod.Any())
                {
                    if (!services.ContainsKey(new KeyValuePair<HttpVerb, Type>(element.Key.Value, element.Value)))
                    {
                        services.Add(
                            new KeyValuePair<HttpVerb, Type>(element.Key.Value, element.Value),
                            matchingMethod.Single().Value.Single(x => x.ReturnType == returnType && x.GetParameters().SingleOrDefault(z => z.ParameterType == element.Value) != null)
                            );
                    }
                }
            }

        }

        public void SetSecurityToReadOnlyForUnkownAuth()
        {
            IsSecurityEnabled = true;
            IsSecuritySetToReadOnlyForUnkownAuth = true;
        }
    }
}