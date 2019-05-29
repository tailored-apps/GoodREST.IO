using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contoso.Foo.WebApi.Messages;
using Contoso.Foo.WebApi.Services;
using GoodREST.Interfaces;
using GoodREST.Middleware;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Contoso.Foo.WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
            services.AddGoodRest(x =>
            {
                x.CharacterEncoding = "utf-8";
            });
            services.AddTransient<IRequestResponseSerializer, GoodREST.Serializers.JsonSerializer>();
            services.AddTransient<ServiceBase, BarService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.TakeGoodRest(configure =>
            {
                configure.RegisterMessageModel<GetBar>();
            });
        }
    }
}
