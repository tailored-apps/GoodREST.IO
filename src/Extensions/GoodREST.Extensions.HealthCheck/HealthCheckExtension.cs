using GoodREST.Middleware.Interface;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace GoodREST.Extensions.HealthCheck
{
    public static class HealthCheckExtensionMethods
    {
        public static void AddHealthCheck(this IServiceCollection services)
        {
            services.AddScoped<ServiceBase, HealthCheckService>();
            services.AddScoped<IExtension, HealthCheckExtension>();
        }
    }

    public class HealthCheckExtension : IExtension
    {
        public void Install(RouteBuilder routeBuilder)
        {
        }
    }
}