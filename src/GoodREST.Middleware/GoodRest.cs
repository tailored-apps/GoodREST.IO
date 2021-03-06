﻿using GoodREST.Extensions;
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

        public static IApplicationBuilder TakeGoodRest(this IApplicationBuilder app, Action<IRestModel> configureRoutes)
        {
            configureRoutes.Invoke(model);
            var scope = app.ApplicationServices.CreateScope();

            model.Build(scope.ServiceProvider.GetServices<ServiceBase>().Select(x => x.GetType()));

            var routeBuilder = new RouteBuilder(app);

            var extension = scope.ServiceProvider.GetServices<IExtension>();
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
                    var requestScope = app.ApplicationServices.CreateScope();
                    var serializer = requestScope.ServiceProvider.GetService<IRequestResponseSerializer>();
                    if (model.IsSecurityEnabled)
                    {
                        IAuthService authService = requestScope.ServiceProvider.GetService<IAuthService>();
                        var verb = route.Key.Value.ToString();
                        var path = route.Key.Key; //context.Request.Path;
                        var headers = context.Request.Headers;
                        string rightsResp = string.Empty;
                        if (!path.Contains(authService.AuthUrl) && CheckRights<object>(authService, model, verb, path, headers, out rightsResp) != null)
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

                    var service = requestScope.ServiceProvider.GetServices<ServiceBase>().Single(x => x.GetType() == method.DeclaringType);
                    service.SecurityService = requestScope.ServiceProvider.GetService<ISecurityService>();
                    var roleAttirbutes = modelTypeInfo.GetCustomAttributes<GoodREST.Annotations.Role>();
                    var authorizationAttirbutes = modelTypeInfo.GetCustomAttribute<GoodREST.Annotations.Authorization>();

                    if (roleAttirbutes != null && roleAttirbutes.Any() || authorizationAttirbutes != null)
                    {
                        var hasValidRole = false;
                        var hasValidAuth = authorizationAttirbutes == null ? true : context.Request.Headers.ContainsKey("X-Auth-Token") && !string.IsNullOrEmpty(service.SecurityService.GetUserId());
                        if (service.SecurityService != null)
                        {

                            if (roleAttirbutes != null && roleAttirbutes.Any())
                            {
                                foreach (var role in roleAttirbutes)
                                {

                                    var roleStatus = role.Codes.All(x => service.SecurityService.IsUserInRole(x));
                                    if (roleStatus)
                                    {
                                        hasValidRole = true;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                hasValidRole = true;
                            }
                        }
                        if (!hasValidRole || !hasValidAuth)
                        {
                            var newResponse = Activator.CreateInstance(method.ReturnType) as IResponse;

                            var correlatedResponse = (newResponse as ICorrelation);
                            if (correlatedResponse != null && req != null)
                            {
                                correlatedResponse.CorrelationId = req.CorrelationId;
                            }
                            if (!hasValidAuth)
                            {
                                newResponse.Unauthorized();
                            }
                            else if (!hasValidRole)
                            {
                                newResponse.Forbidden();
                            }
                            context.Response.ContentType = serializer.ContentType + "; " + model.CharacterEncoding;
                            context.Response.StatusCode = newResponse?.HttpStatusCode ?? context.Response.StatusCode;
                            return context.Response.WriteAsync(serializer.Serialize(newResponse));
                        }
                    }

                    var returnValueFromService = method.Invoke(service, new[] { requestModel });

                    var resp = (returnValueFromService as ICorrelation);
                    if (resp != null && req != null)
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

        private static T CheckRights<T>(IAuthService authService, IRestModel model, string verb, string path, IHeaderDictionary headers, out string resp) where T : class, new()
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