using System;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Wise.goodREST.Middleware.Interface;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Middleware
{
    public static class GoodRest
    {

        public static IServiceCollection AddGoodRest(this IServiceCollection app)
        {
            app.AddTransient<IRestModel, RestModel>();
            
            return app;
        }

        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app, Action<IRestModel> configureRoutes)
        {
            var model = app.ApplicationServices.GetService<IRestModel>();
            configureRoutes.Invoke(model);


            var serializer = app.ApplicationServices.GetService<IRequestResponseSerializer>();

            var trackPackageRouteHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync(
                    $"Hello! Route values: {string.Join(", ", routeValues)}");
            });

            var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);
            model.Build(app.ApplicationServices.GetServices<ServiceBase>().Select(x=>x.GetType()));
            foreach (var route in model.GetRouteForType())
            {
                var template = route.Key.Key;
                string pattern = "{(.*?)}";
                var result = Regex.Matches(template, pattern);
                routeBuilder.MapVerb(route.Key.Value.ToString(), template, context =>
               {
                   var requestmMdel = Activator.CreateInstance(route.Value);

                   var modelTypeInfo = requestmMdel.GetType().GetTypeInfo();
                   if (result != null)
                   {
                       foreach (Match param in result)
                       {
                           var propName = param.Value.Replace("{", string.Empty).Replace("}", string.Empty);
                           modelTypeInfo.GetProperty(propName).SetValue(requestmMdel, context.GetRouteValue(propName));
                       }
                   }
                   context.Items.Add("requestmMdel", requestmMdel);

                   var method = model.GetServiceMethodForType(route.Key.Value, route.Value);
                   var service = app.ApplicationServices.GetServices<ServiceBase>().Single(x=>x.GetType()==method.DeclaringType);
                   var returnValueFromService = method.Invoke(service, new[] { requestmMdel });
                   context.Response.ContentType = serializer.ContentType;
                   return context.Response.WriteAsync(serializer.Serialize(returnValueFromService));
               });
            }



            var routes = routeBuilder.Build();
            app.UseRouter(routes);

            return app;

        }

        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app)
        {
            return app;

        }
    }
}

