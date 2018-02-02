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
using goodREST.Interfaces;
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
                builder.AddUserSecrets<Startup>();
            }
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            //services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //   .AddEntityFrameworkStores<ApplicationDbContext>()
            //  .AddDefaultTokenProviders();

            services.AddRouting();
            services.AddGoodRest(x=> { });
            // Add application services.
            services.AddTransient<IRequestResponseSerializer,goodREST.Serializers.JsonSerializer>();
            services.AddTransient<ServiceBase, CustomerService>();

            services.AddTransient<IExtension, SwaggerExtension>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //    app.UseBrowserLink();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

            //app.UseStaticFiles();

            //app.UseIdentity();

            //// Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715
            
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

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
