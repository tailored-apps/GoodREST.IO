# Swagger UI
## Overview 
That Extension is used for generation swagger UI and swagger schema of current service , general purpose is to generate nice UI which might be used for calling service and for generation service clients from json schema.
## Dependencies 
Package rely on `GoodREST.Middleware` and `GoodREST`
## Instalation
1. Install package from nuget package manager: `Install-Package GoodREST.Extensions.SwaggerExtension` or .NET CLI: `dotnet add package GoodREST.Extensions.SwaggerExtension`
2. Add registration in your `Startup.cs` under:
```
public void ConfigureServices(IServiceCollection services)
{
     ...
     services.AddSwaggerUISupport();
     ...
}
```
## Build And Run Project
For verification purposes if you want to check if swagger ui extension works correctyl, please run your project and go to following url:
`http://{service_url}/swagger/index.html`  where `{service_url}` is url and port where service is running.
After accessing that url you should have Swagger UI Displayed 

## How To use
In case problem with swagger itself, please follow [Swagger](https://swagger.io/)