using GoodREST.Middleware;
using GoodREST.Middleware.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace GoodREST.Extensions.ServiceDiscovery
{
    public static class ServiceDiscoveryExtensionMethod
    {
        public static void AddServiceDiscovery(this IServiceCollection services)
        {
            services.AddTransient<IExtension, ServiceDiscoveryExtension>();
        }
    }

    public class ServiceDiscoveryExtension : IExtension
    {
        private IRestModel restModel;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider services;

        public ServiceDiscoveryExtension(IRestModel restModel,
            IHostingEnvironment env,
            IConfiguration configuration,
            IServiceProvider services)
        {
            this.env = env;
            this.restModel = restModel;
            this.configuration = configuration;
            this.services = services;
        }

        public void Install(RouteBuilder routeBuilder)
        {
            var register = new DataModel.Messages.PostRegisterInServiceDiscovery()
            {
                AppDomainName = env.ApplicationName,
                ApiPrefix = System.Diagnostics.Process.GetCurrentProcess().Container.Components[0].Site.Name,
                Port = 1,
                Version = "",
                ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
                ServiceHost = System.Net.Dns.GetHostName(),
                Operations = restModel
                    .GetRouteForType()
                    .Select(x => new Model.Operation()
                    {
                        Path = x.Key.Key,
                        Verb = x.Key.Value.ToString(),
                        Version = "1"
                    })
                    .ToList()
            };
            Console.Write(register);
        }
    }
}