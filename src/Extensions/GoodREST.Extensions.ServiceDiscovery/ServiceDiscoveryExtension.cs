using GoodREST.Annotations;
using GoodREST.Extensions.ServiceDiscovery.Config;
using GoodREST.Extensions.ServiceDiscovery.DataModel.Messages;
using GoodREST.Middleware;
using GoodREST.Middleware.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        private readonly
            ServiceConfiguration serviceConfiguration;

        public ServiceDiscoveryExtension(IRestModel restModel,
            IHostingEnvironment env,
            IConfiguration configuration,
            IServiceProvider services,
            IOptions<ServiceConfiguration> serviceConfigurationOptions
            )
        {
            this.env = env;
            this.restModel = restModel;
            this.configuration = configuration;
            this.services = services;
            serviceConfiguration = serviceConfigurationOptions.Value;
        }

        public void Install(RouteBuilder routeBuilder)
        {
            var message = new DataModel.Messages.PostRegisterInServiceDiscovery()
            {
                AppDomainName = env.ApplicationName,
                ApiPrefix = serviceConfiguration.Api.Prefix,
                ServiceHost = serviceConfiguration.FromEnvironmentVariable ? GetHostFromEnvironmentVariable() : GetHostFromUri(serviceConfiguration.Endpoint.Url),
                Port = serviceConfiguration.FromEnvironmentVariable ? GetPortFromEnvironmentVariable() : GetPortFromUri(serviceConfiguration.Endpoint.Url),
                SslServiceHost = serviceConfiguration.FromEnvironmentVariable ? GetHostFromEnvironmentVariable() : GetHostFromUri(serviceConfiguration.SecureEndpoint.Url),
                SslPort = serviceConfiguration.FromEnvironmentVariable ? GetHttpsPortFromEnvironment() : GetPortFromUri(serviceConfiguration.SecureEndpoint.Url),
                Version = serviceConfiguration.Api.Version,
                ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id,
                Operations = restModel
                    .GetRouteForType()
                    .Select(x => new Model.Operation()
                    {
                        Path = x.Key.Key,
                        Verb = x.Key.Value.ToString(),
                        Version = GetVersion(x.Value)
                    })
                    .ToList()
            };
            Task.Factory.StartNew(RegisterInDiscroveryService(message));
        }

        private Action RegisterInDiscroveryService(PostRegisterInServiceDiscovery message)
        {
            return () =>
            {
                PostRegisterInServiceDiscoveryResponse post = null;
                while (post == null || post.HttpStatusCode != 200)
                {
                    try
                    {
                        post = new Client.JsonClient(serviceConfiguration.ServiceDiscoveryUrl).Post<PostRegisterInServiceDiscoveryResponse, PostRegisterInServiceDiscovery>(message);
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }
            };
        }

        private string GetVersion(Type type)
        {
            var attrib = type.GetCustomAttributes(typeof(ApiVersion), false).SingleOrDefault() as ApiVersion;

            return attrib?.Version ?? serviceConfiguration.Api.Version;
        }

        private int GetPortFromUri(string uri)
        {
            return new Uri(uri).Port;
        }

        private string GetHostFromUri(string uri)
        {
            return new Uri(uri).Host;
        }

        private int? GetHttpsPortFromEnvironment()
        {
            int port = -1;
            if (int.TryParse(Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT"), out port))
            {
                return port;
            }
            return null;
        }

        private int GetPortFromEnvironmentVariable()
        {
            try
            {
                return GetPortFromUri(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
            }
            catch (Exception)
            {
                throw new Exception("Can't parse system variable 'ASPNETCORE_URLS' ");
            }
        }

        private string GetHostFromEnvironmentVariable()
        {
            try
            {
                return GetHostFromUri(Environment.GetEnvironmentVariable("ASPNETCORE_URLS"));
            }
            catch (Exception)
            {
                throw new Exception("Can't parse system variable 'ASPNETCORE_URLS' ");
            }
        }
    }
}