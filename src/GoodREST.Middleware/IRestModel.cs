using System;
using System.Collections.Generic;
using System.Reflection;
using GoodREST.Enums;

namespace GoodREST.Middleware
{
    public interface IRestModel
    {
        bool IsSecurityEnabled { get; }
        bool IsSecuritySetToReadOnlyForUnkownAuth { get; }
		string CharacterEncoding { get; }
        void SetSecurityToReadOnlyForUnkownAuth();
        void Build(IEnumerable<Type> registeredServices);
        Dictionary<KeyValuePair<string, HttpVerb>, Type> GetRouteForType();
        void RegisterMessageModel<T>();
        MethodInfo GetServiceMethodForType(HttpVerb verb, Type requestType);
    }
}