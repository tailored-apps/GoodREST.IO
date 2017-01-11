using System;
using System.Collections.Generic;
using System.Reflection;
using Wise.goodREST.Core.Enums;
using Wise.goodREST.Core.Interfaces;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Middleware
{
    public interface IRestModel
    {
        void Build(IEnumerable<Type> registeredServices);
        Dictionary<KeyValuePair<string, HttpVerb>, Type> GetRouteForType();
       void  RegisterMessageModel<T>() ;
        MethodInfo GetServiceMethodForType(HttpVerb verb, Type requestType);
       // void RegisterSercice<T>() where T : ServiceBase;
    }
}