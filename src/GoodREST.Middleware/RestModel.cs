﻿using GoodREST.Annotations;
using GoodREST.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GoodREST.Middleware
{
    public class RestModel : IRestModel
    {
        public bool IsSecurityEnabled { get; private set; }
        public List<string> SecuirtyExcludedPaths { get; private set; }
        public bool IsSecuritySetToReadOnlyForUnkownAuth { get; private set; }
        public string CharacterEncoding { get; set; }
        private Dictionary<KeyValuePair<string, HttpVerb>, Type> types = new Dictionary<KeyValuePair<string, HttpVerb>, Type>();
        private Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo> services = new Dictionary<KeyValuePair<HttpVerb, Type>, MethodInfo>();

        private Dictionary<Type, IList<MethodInfo>> serviceMethods = new Dictionary<Type, IList<MethodInfo>>();
        private IEnumerable<Type> registeredServices;

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
            this.registeredServices = registeredServices;

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

        public Type[] GetServices()
        {
            return registeredServices.ToArray();
        }
    }
}