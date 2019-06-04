# GoodREST.Extensions.HealthCheck  
## Overview 
That Extension is used for checking if service responds for request, general purpose is to check instance 
of service is still alive, othervise we can recycle that instance and initiate it again
## Dependencies 
Package rely on `GoodREST.Middleware` and `GoodREST`
## Instalation
1. Install package from nuget package manager: `Install-Package GoodREST.Extensions.HealtCheck` or .NET CLI: `dotnet add package GoodREST.Extensions.HealthCheck`
2. Add registration in your `Startup.cs` under:
```
public void ConfigureServices(IServiceCollection services)
{
     ...
     services.AddHealthCheck();
     ...
}
``` 
3. Register message in configure method:
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    app.TakeGoodRest(configure =>
    {
        ...
        configure.RegisterMessageModel<GetHealthCheck>();
        ...
    });
}
```
## Build And Run Project
For verification purposes if you want to check if health check service works correctyl run your project and go to following url:
`http://{service_url}/healthcheck`  where `{service_url}` is url and port where service is running.
After accessing that url you should get following response:
```
```