# Simple Example
Service classes are used for orchestration service behaviour. Service classes have to inherit from ServiceBase class by what GoodRest will know how to read that class.

Below its shown BarService from QuickStartExample , you can find that service class is corresponding to domain area and method name is corresponding to operation Verb. 
this is our suggested design pattern which we follow in our projects. if you have nicer idea you might name it different way.
```
using System;
using Contoso.Foo.WebApi.Messages;
using GoodREST.Extensions;
using GoodREST.Middleware.Services;

namespace Contoso.Foo.WebApi.Services
{
    public class BarService : ServiceBase
    {
        public GetBarResponse Get(GetBar request)
        {
            var response = new GetBarResponse();
            try
            {
                response.FooBar = "It Works!";
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }
    }
}
```
Clue of service method is to have Public method which returns object which inherits from `IResponse` interface and same method should have argument of type which implements `IHasResponse<T> where T : IResponse` and type of `T` must be same type as method return object type ;)


Service Dependency injection is realized by constructor so if for example your service is using some database context you can use constructor arguments for passing dependencies. 
```
using System;
using Contoso.Foo.WebApi.Messages;
using GoodREST.Extensions;
using GoodREST.Middleware.Services;

namespace Contoso.Foo.WebApi.Services
{
    public class BarService : ServiceBase
    {
        private readonly IDataRepository repository;
        public BarService(IDataRepository repository)
        {
            this.repository=repository;
        }
        public GetBarResponse Get(GetBar request)
        {
            var response = new GetBarResponse();
            try
            {
                response.FooBar = this.repository.GetBar();
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }
    }
}
```