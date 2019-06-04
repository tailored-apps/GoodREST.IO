using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Core.Test.Services;
using GoodREST.Extensions.HealthCheck;
using GoodREST.Extensions.HealthCheck.Messages;
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
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddSwaggerUISupport();
            services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();
            services.AddScoped<ServiceBase, CustomerService>();
            services.AddScoped<IMockingRepository, MoqRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.TakeGoodRest(configure =>
            {
                configure.RegisterMessageModel<GetHealthCheck>();
                configure.RegisterMessageModel<GetCustomers>();
                configure.RegisterMessageModel<PostCustomer>();
                configure.RegisterMessageModel<PutCustomer>();
                configure.RegisterMessageModel<DeleteCustomer>();
            });
        }
    }
}