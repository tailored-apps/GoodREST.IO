using System;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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

            routeBuilder.MapRoute(
                "Track Package Route",
                "package/{operation:regex(^track|create|detonate$)}/{id:int}");

            foreach (var route in model.GetRouteForType())
            {
                routeBuilder.MapVerb( route.Key.Value.ToString(), route.Key.Key, context =>
                {
                    //var name = context.GetRouteValue("name");
                    // This is the route handler when HTTP GET "hello/<anything>"  matches
                    // To match HTTP GET "hello/<anything>/<anything>, 
                    // use routeBuilder.MapGet("hello/{*name}"
                    return context.Response.WriteAsync($"Hi, {route.Key.Key}!");
                });
            }

            routeBuilder.MapGet("hello/{name}", context =>
            {
                var name = context.GetRouteValue("name");
                // This is the route handler when HTTP GET "hello/<anything>"  matches
                // To match HTTP GET "hello/<anything>/<anything>, 
                // use routeBuilder.MapGet("hello/{*name}"
                return context.Response.WriteAsync($"Hi, {name}!");
            });

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

