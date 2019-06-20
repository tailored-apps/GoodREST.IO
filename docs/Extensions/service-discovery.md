# Service Discovery  
## Overview 
That Extension is used for Hosting and registering service discovery in your microservices architecture This functionality gives you ability of registering all services in discovery service.
That registrations can be used by ApiGetaway so your application automatically will be scallable and will allow you to create scallable architecture.
## Dependencies 
Package rely on `GoodREST.Middleware` and `GoodREST`
## Instalation
1. Install package from nuget package manager: `Install-Package GoodREST.Extensions.ServiceDiscovery` or .NET CLI: `dotnet add package GoodREST.Extensions.ServiceDiscovery`
2. Add configuration file `servicediscovery.json` and add registration in your `Startup.cs` under:
```
public Startup(IHostingEnvironment env)
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile($"Config/servicediscovery.json", optional: true, reloadOnChange: true);

        if (env.IsDevelopment())
        {
            builder.AddUserSecrets<Startup>();
        }
        builder.AddEnvironmentVariables();
        Configuration = builder.Build();
}

public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddServiceDiscovery();
    ...
}
``` 
3. For Service Discovery host Register message in configure method:
```
public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
{
    app.TakeGoodRest(configure =>
    {
        ...
        configure.RegisterMessageModel<GetAllOperations>();
        configure.RegisterMessageModel<GetAllRegisteredServices>();
        configure.RegisterMessageModel<PostRegisterInServiceDiscovery>();
        ...
    });
}
```
4. Build Config file
Services can be hosted on various options on IIS , by Kestrel in console or in DOCKER image,
Just for purpose of right registartion in service discovery you have to create proper `servicediscovery.json` configuration file.
Example body of config file can be similar to this:
```
{
  "ServiceDiscovery": {
    "Endpoint": {
      "Url": "http://localhost:8080"
    },
    "SecureEndpoint": {
      "Url": "https://localhost:8081"
    },
    "Api": {
      "Prefix": "demo",
      "Version": "1"
    },
    "FromEnvironmentVariable": true,
    "ServiceDiscoveryUrl": "http://localhost:8080"
  }
}
```
`ServiceDiscoveryUrl` - is used for defining URL of service discovery service.

`FromEnvironmentVariable` - drives if you would like to take environmental variables as sources of Endpoints

`Api.Prefix` - will be used as prefix for service which register operations in servicediscovery

`Api.Version` - is used for defining api varsion  


## Build And Run Project
For verification purposes if you want to check if health check service works correctyl run your service discovery host and verify registered operations by executing `operations` action on API 
```
```