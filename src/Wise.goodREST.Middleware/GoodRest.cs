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
using System.Collections.Generic;
using System.IO;
using System.Text;
using goodREST.Interfaces;

namespace Wise.goodREST.Middleware
{
    public static class GoodRest
    {
        private static RestModel model;
        public static IServiceCollection AddGoodRest(this IServiceCollection app, Action<RestModel> action)
        {
             model = new RestModel();
            action.Invoke(model);

            app.AddScoped<IRestModel>(x=> { return model; });

            return app;
        }

        private static ISecurityService securityService;
        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app, Action<IRestModel> configureRoutes)
        {
            //var model = app.ApplicationServices.GetService<IRestModel>();
            configureRoutes.Invoke(model);
            
            securityService = app.ApplicationServices.GetService<ISecurityService>();
            var extension = app.ApplicationServices.GetServices<IExtension>();

            var serializer = app.ApplicationServices.GetService<IRequestResponseSerializer>();

            var trackPackageRouteHandler = new RouteHandler(context =>
            {
                var routeValues = context.GetRouteData().Values;
                return context.Response.WriteAsync(
                    $"Hello! Route values: {string.Join(", ", routeValues)}");
            });

            model.Build(app.ApplicationServices.CreateScope().ServiceProvider.GetServices<ServiceBase>().Select(x => x.GetType()));

            var routeBuilder = new RouteBuilder(app, trackPackageRouteHandler);

            routeBuilder.MapGet("", conext =>
            {
                var urls = new StringBuilder();

                urls.AppendLine("<html>");
                urls.AppendLine("<body>");
                urls.Append("<h3>API LIST:</h3>");
                urls.AppendLine("<table>");
                urls.AppendFormat(@"<tr><th>Path</th> <th>Operation</th> <th>Message</th></tr>");
                foreach (var route in model.GetRouteForType().OrderBy(x => x.Key.Key))
                {
                    urls.AppendFormat(@"<tr><td>{0}</td> <td>{1}</td> <td>{2}</td></tr>", route.Key.Key, route.Key.Value , route.Value.FullName);

                }
                urls.AppendLine("</table>");

                urls.AppendLine("</body>");
                urls.AppendLine("</html>");
                
                conext.Response.ContentType = "text/html; charset=UTF-8";
                
                return conext.Response.WriteAsync(urls.ToString());
            });

            if (extension != null && extension.Any())
            {
                foreach (var ext in extension)
                {
                    ext.Install(routeBuilder);

                }
            }

            foreach (var route in model.GetRouteForType())
            {
                var template = route.Key.Key;
                string pattern = "{(.*?)}";
                var result = Regex.Matches(template, pattern);
                routeBuilder.MapVerb(route.Key.Value.ToString(), template, context =>
               {

                   if (model.IsSecurityEnabled)
                   {
                       var verb = route.Key.Value.ToString();
                       var path = route.Key.Key; //context.Request.Path;
                       var headers = context.Request.Headers;
                       string rightsResp = string.Empty;
                       if (!path.Contains("Auth") && !CheckRights(model, verb, path, headers, out rightsResp))
                       {
                           return context.Response.WriteAsync(rightsResp);
                       };
                   }

                   var requestModel = serializer.Deserialize(route.Value, new StreamReader(context.Request.Body).ReadToEnd()) ?? Activator.CreateInstance(route.Value);

                   var modelTypeInfo = requestModel.GetType().GetTypeInfo();

                   if (result != null)
                   {
                       foreach (Match param in result)
                       {
                           var propName = param.Value.Replace("{", string.Empty).Replace("}", string.Empty);
                           modelTypeInfo.GetProperty(propName).SetValue(requestModel, Convert.ChangeType(context.GetRouteValue(propName), modelTypeInfo.GetProperty(propName).PropertyType));
                       }
                   }

                   var req = (requestModel as ICorrelation);
                   if (req != null && string.IsNullOrWhiteSpace(req.CorrelationId))
                   {
                       req.CorrelationId = Guid.NewGuid().ToString();
                   }

                   context.Items.Add("requestModel", requestModel);
                   var method = model.GetServiceMethodForType(route.Key.Value, route.Value);
                   var service = app.ApplicationServices.CreateScope().ServiceProvider.GetServices<ServiceBase>().Single(x => x.GetType() == method.DeclaringType);
                   service.GetType().GetTypeInfo().GetProperty("Context").SetValue(service, context);
                   var returnValueFromService = method.Invoke(service, new[] { requestModel });



                   var resp = (returnValueFromService as ICorrelation);
                   if (resp != null)
                   {
                       resp.CorrelationId = req.CorrelationId;
                   }

                   context.Response.ContentType = serializer.ContentType;

                   return context.Response.WriteAsync(serializer.Serialize(returnValueFromService));
               });
            }



            var routes = routeBuilder.Build();
            app.UseRouter(routes);

            return app;

        }

        private static bool CheckRights(IRestModel model, string verb, string path, IHeaderDictionary headers, out string resp)
        {
            resp = string.Empty;
            if (model.IsSecuritySetToReadOnlyForUnkownAuth && verb == "GET")
            {
                return true;
            }

            var token = headers.SingleOrDefault(x => x.Key == "X-Auth-Token").Value;

            if (securityService == null)
            {
                throw new Exception("ISecurityService not registered");
            }
            return securityService.CheckAccess(token);

            resp = "NoRights";
            return false;
        }
        
        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app)
        {
            return app;

        }
    }
}

