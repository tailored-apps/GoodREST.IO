using GoodREST.Annotations;
using GoodREST.Extensions.SwaggerExtension.Auxillary;
using GoodREST.Interfaces;
using GoodREST.Middleware;
using GoodREST.Middleware.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoodREST.Extensions.SwaggerExtension
{
    public static class SwaggerExtensionMethod
    {
        public static void AddSwaggerUISupport(this IServiceCollection services)
        {
            services.AddTransient<IExtension, SwaggerExtension>();
        }
    }

    public class SwaggerExtension : IExtension
    {
        private IRestModel restModel;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider services;
        private readonly IOptions<info> optionsInfo;
        private readonly IOptions<externalDocs> externalDocs;

        public SwaggerExtension(IRestModel restModel,
            IHostingEnvironment env,
            IConfiguration configuration,
            IServiceProvider services,
            IOptions<info> optionsInfo,
            IOptions<externalDocs> externalDocs)
        {
            this.env = env;
            this.restModel = restModel;
            this.configuration = configuration;
            this.services = services;
            this.optionsInfo = optionsInfo;
            this.externalDocs = externalDocs;
        }

        private Swagger BuildServiceSchema()
        {
            var swaggerDefinition = new Swagger
            {
                swagger = "2.0",

                #region info

                info = optionsInfo.Value,

                #endregion info

                host = null,
                basePath = @"/",
                schemes = new[] { "http", "https" },

                externalDocs = (!string.IsNullOrWhiteSpace(externalDocs.Value?.url) ? externalDocs.Value : null)
            };

            if (string.IsNullOrWhiteSpace(swaggerDefinition.info.title))
            {
                swaggerDefinition.info.title = AppDomain.CurrentDomain.FriendlyName;
            }
            if (string.IsNullOrWhiteSpace(swaggerDefinition.info.version))
            {
                swaggerDefinition.info.version = "1.0.0";
            }

            #region objectDefinitions

            var objectDefiniton = GenerateObjectDefinition(restModel);
            foreach (var obj in objectDefiniton)
            {
                swaggerDefinition.AddObjectDefinition(obj.Item1, obj.Item2);
            }

            #endregion objectDefinitions

            #region tagDefinition

            var serviceDefinition = restModel.GetServices();
            foreach (var service in serviceDefinition)
            {
                swaggerDefinition.AddTag(service.GetServiceTag());
            }

            #endregion tagDefinition

            #region securityDefinition

            if (this.restModel.IsSecurityEnabled)
            {
                swaggerDefinition.AddSecurityDefinition("X-Auth-Token", new securityDefinitionInfo
                {
                    type = "apiKey",
                    name = "X-Auth-Token",
                    @in = "header"
                });
            }

            #endregion securityDefinition

            var contentTypes = services.GetServices<IRequestResponseSerializer>().Select(x => x.ContentType);

            #region pathDescription

            foreach (var item in restModel.GetRouteForType())
            {
                var method = restModel.GetServiceMethodForType(item.Key.Value, item.Value);

                var @pathDesc = method.GetMessageTag(item.Value, item.Value.Name, contentTypes);
                if (!(item.Key.Value == Enums.HttpVerb.GET && this.restModel.IsSecuritySetToReadOnlyForUnkownAuth) && this.restModel.IsSecurityEnabled)
                {
                    pathDesc.AddSecurity(new verbSecurity { value = "X-Auth-Token", operations = new string[0] });
                }
                var parts = item.Key.Key.GetPathParts();
                foreach (var parameter in parts)
                {
                    pathDesc.AddParameter(new parameter
                    {
                        @in = "path",
                        name = parameter,
                        description = "The " + parameter + " key",
                        required = true,
                        type = item.Value.GetProperty(parameter).PropertyType.GetJavascriptType()
                    });
                }

                if (item.Key.Value != Enums.HttpVerb.GET)
                {
                    pathDesc.AddParameter(new parameter
                    {
                        @in = "body",
                        name = "body",
                        description = "The " + item.Value.Name + " key",
                        required = true,
                        schema = new Dictionary<string, string> { { "$ref", "#/definitions/" + item.Value.Name } }
                    });
                }
                swaggerDefinition.AddOperation(@"/" + item.Key.Key, item.Key.Value.ToString().ToLower(), pathDesc);
            }

            #endregion pathDescription

            return swaggerDefinition;
        }

        private IEnumerable<(string, objectDefiniton)> GenerateObjectDefinition(IRestModel model)
        {
            List<Type> created = new List<Type>();
            foreach (var modelToRegister in model.GetRouteForType())
            {
                created.AddNotExistingRange(modelToRegister.Value.GetTypeTree());
            }
            foreach (var type in created)
            {
                if (type.IsEnum)
                {
                    var props = new property()
                    {
                        name=type.Name,
                        propertyDescription = new Dictionary<string, object>() { { "type", type.GetJavascriptType() }, { "enum", Enum.GetNames(type) } }

                    };

                    var objectDefinition = new objectDefiniton
                    {
                        properties = new List<property>() { props }
                    };

                    yield return (type.Name, objectDefinition);

                }
                else if (type != typeof(byte) && type != typeof(byte[]))
                {
                    var allProperties = type.GetProperties();
                    var properties = allProperties.Select(x =>
                {
                    return new property()
                    {
                        name = x.Name,
                        propertyDescription = x.PropertyType.GetPropertyDescription(),
                    };

                }).ToList();
                    var reqired = allProperties.Where(x => x.PropertyType.IsRequired()).Select(x => x.Name).ToList();

                    var objectDefinition = new objectDefiniton
                    {
                        properties = properties,
                        RequiredProperties = reqired
                    };

                    yield return (type.Name, objectDefinition);
                }
            }
        }

        public void Install(RouteBuilder routeBuilder)
        {
            routeBuilder.MapGet(@"swagger/serviceSchema.json", conext =>
            {
                var schemaDefinition = BuildServiceSchema();
                return conext.SwaggerSchema(schemaDefinition);
            });
            routeBuilder.MapGet(@"swagger/{url}", conext =>
            {
                return conext.SwaggerUI();
            });
            routeBuilder.MapGet(@"swagger/{url}/{subdir}", conext =>
            {
                return conext.SwaggerUI();
            });
        }
    }
}