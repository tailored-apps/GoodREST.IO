using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GoodREST.Middleware;
using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Core.Test.Services;
using GoodREST.Middleware.Services;
using GoodREST.Middleware.Interface;
using GoodREST.Interfaces;
using GoodREST.Extensions.SwaggerExtension;

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
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddRouting();
            services.AddGoodRest(x => { });
            // Add application services.
            services.AddTransient<IExtension, SwaggerExtension>();
            services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();
            services.AddTransient<ServiceBase, CustomerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.TakeGoodRest(configure =>
            {
                configure.RegisterMessageModel<GetCustomer>();
                configure.RegisterMessageModel<PostCustomer>();
                configure.RegisterMessageModel<PutCustomer>();
                configure.RegisterMessageModel<DeleteCustomer>();
            });
        }
    }
}
