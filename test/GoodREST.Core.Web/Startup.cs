using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Core.Test.Services;
using GoodREST.Core.Web;
using GoodREST.Extensions.HealthCheck;
using GoodREST.Extensions.HealthCheck.Messages;
using GoodREST.Extensions.ServiceDiscovery;
using GoodREST.Extensions.ServiceDiscovery.Config;
using GoodREST.Extensions.ServiceDiscovery.DataModel.Messages;
using GoodREST.Extensions.ServiceDiscovery.Middleware.Services;
using GoodREST.Extensions.SwaggerExtension;
using GoodREST.Interfaces;
using GoodREST.Middleware;
using GoodREST.Middleware.Interface;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"Config/appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"Config/servicediscovery.json", optional: true, reloadOnChange: true);

            if (env.EnvironmentName != "PROD")
            {
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var serviceDiscovery = Configuration.GetSection("ServiceDiscovery");
            services.AddOptions<ServiceConfiguration>().Bind(serviceDiscovery);

            services.AddRouting();
            services.AddGoodRest(x =>
            {
                x.CharacterEncoding = "utf-8";
            });
            // Add application services.
            services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();
            services.AddGoodRest(x =>
            {
                x.CharacterEncoding = "utf-8";
            });

            services.AddHealthCheck();
            services.AddServiceDiscovery();
            services.AddSwaggerUISupport();
            services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();

            services.AddScoped<IServiceInfoPersister, InMemoryServiceInfoPersister>();
            services.AddScoped<ServiceBase, CustomerService>();
            services.AddScoped<ServiceBase, ServiceDiscoveryService>();
            services.AddScoped<IMockingRepository, MoqRepository>();
            services.AddScoped<ISecurityService, MockedSecurityService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.TakeGoodRest(configure =>
            {
                configure.RegisterMessageModel<GetHealthCheck>();
                configure.RegisterMessageModel<GetCustomers>();
                configure.RegisterMessageModel<PostCustomer>();
                configure.RegisterMessageModel<PutCustomer>();
                configure.RegisterMessageModel<DeleteCustomer>();
                configure.RegisterMessageModel<GetAllOperations>();
                configure.RegisterMessageModel<GetAllRegisteredServices>();
                configure.RegisterMessageModel<PostRegisterInServiceDiscovery>();
            });
        }
    }
}