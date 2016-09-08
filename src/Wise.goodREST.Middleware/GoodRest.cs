using System;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace Wise.goodREST.Middleware
{
    public static class GoodRest
    {
        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app, Action<IRestModel> configureRoutes)
        {
            var model = new RestModel();
            configureRoutes.Invoke(model);

            

            var trackPackageRouteHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync(
                    $"Hello! Route values: {string.Join(", ", routeValues)}");
            });

            var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);
            
            foreach (var route in model.GetRouteForType())
            {
                var template = route.Key.Key;
                string pattern = "{(.*?)}";
               var result = Regex.Matches(template, pattern);
                routeBuilder.MapVerb(route.Key.Value.ToString(), template, context =>
               {
                   var requestmMdel = Activator.CreateInstance(route.Value);

                   var modelTypeInfo = requestmMdel.GetType().GetTypeInfo();
                   if (result != null )
                   {
                       foreach (Match param in result)
                       {
                           var propName = param.Value.Replace("{", string.Empty).Replace("}", string.Empty); 
                           modelTypeInfo.GetProperty(propName).SetValue(requestmMdel, context.GetRouteValue(propName));
                       }
                   }
                   context.Items.Add("requestmMdel", requestmMdel);


                   context.Response.ContentType = "application/json";
                   
                   return context.Response.WriteAsync(JsonConvert.SerializeObject(requestmMdel));
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

