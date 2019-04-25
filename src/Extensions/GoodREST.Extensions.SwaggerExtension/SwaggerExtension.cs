using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using GoodREST.Middleware;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using GoodREST.Middleware.Interface;
using GoodREST.Middleware;
using Microsoft.Extensions.Configuration;
using GoodREST.Extensions.SwaggerExtension.Auxillary;
using Microsoft.Extensions.DependencyInjection;
using GoodREST.Interfaces;
using Microsoft.Extensions.Options;

namespace GoodREST.Extensions.SwaggerExtension
{
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

                externalDocs = externalDocs.Value
            };

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
                        required = true
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
                var properties = type.GetProperties().Select(x =>
                {
                    return new property()
                    {
                        name = x.Name,
                        propertyDescription = x.PropertyType.GetPropertyDescription()
                    };
                }).ToList();

                var objectDefinition = new objectDefiniton
                {
                    properties = properties
                };

                yield return (type.Name, objectDefinition);


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

