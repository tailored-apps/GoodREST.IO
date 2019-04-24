using GoodREST.Interfaces;
using GoodREST.Middleware.Interface;
using GoodREST.Middleware.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GoodREST.Middleware
{
    public static class GoodRest
    {
        private static RestModel model;
        public static IServiceCollection AddGoodRest(this IServiceCollection app, Action<RestModel> action)
        {
            model = new RestModel();
            action.Invoke(model);

            app.AddScoped<IRestModel>(x => { return model; });

            app.AddScoped<IRequestProvider, RequestProvider>();
            return app;
        }

        private static IAuthService authService;
        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app, Action<IRestModel> configureRoutes)
        {
            //var model = app.ApplicationServices.GetService<IRestModel>();
            configureRoutes.Invoke(model);
            using (var scope = app.ApplicationServices.CreateScope())
            {
                authService = scope.ServiceProvider.GetService<IAuthService>();
                var extension = scope.ServiceProvider.GetServices<IExtension>();

                var serializer = scope.ServiceProvider.GetService<IRequestResponseSerializer>();


                model.Build(scope.ServiceProvider.GetServices<ServiceBase>().Select(x => x.GetType()));

                var routeBuilder = new RouteBuilder(app);

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
                        urls.AppendFormat(@"<tr><td>{0}</td> <td>{1}</td> <td>{2}</td></tr>", route.Key.Key, route.Key.Value, route.Value.FullName);

                    }
                    urls.AppendLine("</table>");

                    urls.AppendLine("</body>");
                    urls.AppendLine("</html>");

                    conext.Response.ContentType = "text/html; " + model.CharacterEncoding;

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
                           if (!path.Contains(authService.AuthUrl) && CheckRights<object>(model, verb, path, headers, out rightsResp) != null)
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
                       var scopedSerciceProvider = scope.ServiceProvider;


                       var service = scopedSerciceProvider.GetServices<ServiceBase>().Single(x => x.GetType() == method.DeclaringType);
                       service.SecurityService = scopedSerciceProvider.GetService<ISecurityService>();
                       var returnValueFromService = method.Invoke(service, new[] { requestModel });



                       var resp = (returnValueFromService as ICorrelation);
                       if (resp != null)
                       {
                           resp.CorrelationId = req.CorrelationId;
                       }
                       var iResponse = returnValueFromService as IResponse;

                       context.Response.ContentType = serializer.ContentType + "; " + model.CharacterEncoding;
                       context.Response.StatusCode = iResponse?.HttpStatusCode ?? context.Response.StatusCode;
                       return context.Response.WriteAsync(serializer.Serialize(returnValueFromService));
                   });
                }



                var routes = routeBuilder.Build();
                app.UseRouter(routes);

                return app;
            }
        }

        private static T CheckRights<T>(IRestModel model, string verb, string path, IHeaderDictionary headers, out string resp) where T : class, new()
        {
            resp = string.Empty;
            if (model.IsSecuritySetToReadOnlyForUnkownAuth && verb == "GET")
            {
                return new T();
            }

            var token = headers.SingleOrDefault(x => x.Key == "X-Auth-Token").Value;

            if (authService == null)
            {
                throw new Exception("IAuthService not registered");
            }
            return authService.CheckAccess<T>(token);

        }

        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app)
        {
            return app;

        }
    }
}

