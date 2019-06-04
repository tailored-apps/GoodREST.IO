# AddGoodRest

GoodREST.IO library is based on Middleware pattern used by ASP.NET Core. Services can be used with all possible lifetime scopes available in framework.

## Register Middleware in WebAPI project.
First things firs , for having working GoodREST.IO library you have to use ASP.NET project so you get all required setup for web app.
Next step is to reference GoodREST by adding using statements:
```
using GoodREST.Interfaces;
using GoodREST.Middleware;
using GoodREST.Middleware.Services;
```
in ConfigureServices method you have to add following :
```
services.AddRouting();
services.AddGoodRest(x =>
{
    x.CharacterEncoding = "utf-8";
});
services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();

services.AddScoped<ServiceBase, BarService>();
```
where `services.AddScoped<ServiceBase, BarService>(); ` is responsible for registration your [Service classes](../Services/simple-service.md)

under `Configure` method last step is to register all messages used by service 
```
app.TakeGoodRest(configure =>
{
    configure.RegisterMessageModel<GetBar>();
});
``` 
and your request now can be handled by services correctly.
