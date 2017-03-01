using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wise.goodREST.Middleware;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Wise.goodREST.Core.Test.Services;
using Wise.goodREST.Middleware.Services;
using Wise.goodREST.Middleware.Interface;
using Wise.goodREST.Core.Interface;
using Wise.goodREST.Extensions.SwaggerExtension;

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
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddRouting();
            services.AddGoodRest();
            // Add application services.
            services.AddTransient<IExtension, SwaggerExtension>();
            services.AddTransient<IRequestResponseSerializer, Wise.goodREST.Core.Serializers.JsonSerializer>();
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