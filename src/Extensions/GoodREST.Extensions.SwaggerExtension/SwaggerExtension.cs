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
using GoodREST.Annotations;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using GoodREST.Interfaces;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class SwaggerExtension : IExtension
    {
        private IRestModel model;
        private Swagger swaggerDefinition;
        private readonly IHostingEnvironment env;
        private readonly IConfiguration configuration;
        public SwaggerExtension(IRestModel restModel, IHostingEnvironment env, IConfiguration configuration)
        {
            this.env = env;
            model = restModel;
            this.configuration = configuration;


        }
        private void BuildServiceSchema()
        {
            swaggerDefinition = new Swagger
            {

                swagger = "2.0",
                #region info
                info = new info
                {

                    description = "This is a sample server Petstore server.  You can find out more about Swagger at [http://swagger.io](http://swagger.io) or on [irc.freenode.net, #swagger](http://swagger.io/irc/).  For this sample, you can use the api key `special-key` to test the authorization filters.",
                    version = typeof(Swagger).GetTypeInfo().Assembly.ImageRuntimeVersion,
                    title = "Swagger Petstore",
                    termsOfService = "http://swagger.io/terms/",
                    contact = new contact { email = "apiteam@swagger.io" },
                    license = new license { name = "Apache 2.0", url = "http://www.apache.org/licenses/LICENSE-2.0.html" }
                },
                #endregion info
                host = null,
                basePath = @"/",
                schemes = new[] { "http" },

                externalDocs = new externalDocs { description = "Find out more about Swagger", url = "http://swagger.io" }
            };

            #region objectDefinitions
            List<Type> created = new List<Type>();
            foreach (var modelToRegister in model.GetRouteForType())
            {

                var registeredRequest = modelToRegister.Value;
                var returnResponse = registeredRequest.GetInterfaces().Single(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IHasResponse<>))).GenericTypeArguments.Single();


                var properties = modelToRegister.Value.GetProperties().Select(x =>
                {
                    return new property()
                    {
                        name = x.Name,
                        propertyDescription = x.PropertyType.GetPropertyDescription(swaggerDefinition, created)

                    };
                }).ToList();
                properties.Add(
                new property()
                {
                    name = returnResponse.Name,
                    propertyDescription = returnResponse.GetPropertyDescription(swaggerDefinition, created)

                });

                var objectDefinition = new objectDefiniton
                {
                    properties = properties
                };

                swaggerDefinition.AddObjectDefinition(registeredRequest.Name, objectDefinition);

            }
            #endregion objectDefinitions

            #region tagDefinition
            swaggerDefinition.AddTag(new tag { name = "pet", description = "Everything about your Pets", externalDocs = new doc { description = "Find out more", url = "http://swagger.io" } });
            swaggerDefinition.AddTag(new tag { name = "store", description = "Access to Petstore orders" });
            swaggerDefinition.AddTag(new tag { name = "user", description = "Operations about user", externalDocs = new doc { description = "Find out more about our store", url = "http://swagger.io" } });
            #endregion tagDefinition
            #region securityDefinition
            swaggerDefinition.AddSecurityDefinition("petstore_auth", new securityDefinitionInfo
            {
                type = "oauth2",
                authorizationUrl = "http://petstore.swagger.io/oauth/dialog",
                flow = "implicit",
                scopes = new Dictionary<string, string>
                {
                    { "write:pets", "modify pets in your account" }
                }

            });

            swaggerDefinition.AddSecurityDefinition("api_key", new securityDefinitionInfo
            {
                type = "apiKey",
                name = "api_key",
                @in = "header"
            });


            #endregion securityDefinition
            #region pathDescription
            foreach (var item in model.GetRouteForType())
            {
                var method = model.GetServiceMethodForType(item.Key.Value, item.Value);

                var @pathDesc = new pathDescription
                {
                    tags = new[] { item.Key.Key.Split("/").FirstOrDefault() },
                    summary = method.GetAttribute<ServiceDescriptionAttribute>()?.Description,
                    description = method.GetAttribute<ServiceDescriptionAttribute>()?.Description,
                    operationId = item.Value.Name,
                    consumes = new[] { "application/json", "application/xml" },
                    produces = new[] { "application/xml", "application/json" },
                };

                pathDesc.AddSecurity(new verbSecurity { value = "petstore_auth", operations = new[] { "write:pets", "read:pets" } });
                pathDesc.AddResponse(new response { code = "405", description = new responseDescription { description = "Invalid input" } });
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
        }

        private Task Swagger(HttpContext builder)
        {

            var assembly = typeof(SwaggerExtension).GetTypeInfo().Assembly;
            var requestResourceName = @"GoodREST.Extensions.SwaggerExtension.swagger" + builder.Request.Path.Value.Replace(@"swagger/", string.Empty).Replace("/", ".");
            var resourceStream = assembly.GetManifestResourceStream(requestResourceName);


            if (requestResourceName.EndsWith("png"))
            {

                builder.Response.ContentType = "data:image/png;base64";

                return builder.Response.WriteAsync(ConvertToBase64(resourceStream));
            }
            else if (requestResourceName.EndsWith("gif"))
            {

                builder.Response.ContentType = "data:image/gif;base64";
                return builder.Response.WriteAsync(ConvertToBase64(resourceStream));
            }
            else
            {
                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {

                    if (requestResourceName.EndsWith("css"))
                    {

                        builder.Response.ContentType = "text/css; charset=UTF-8";
                    }
                    else if (requestResourceName.EndsWith("ttf"))
                    {

                        builder.Response.ContentType = "font/opentype";
                    }
                    else if (requestResourceName.EndsWith("json"))
                    {

                        builder.Response.ContentType = "text/json; charset=UTF-8";
                    }
                    else
                    {

                        builder.Response.ContentType = "text/html; charset=UTF-8";
                    }
                    return builder.Response.WriteAsync(reader.ReadToEnd());
                }
            }
        }
        public string ConvertToBase64(Stream stream)
        {
            Byte[] inArray = new Byte[(int)stream.Length];
            Char[] outArray = new Char[(int)(stream.Length * 1.34)];
            stream.Read(inArray, 0, (int)stream.Length);
            Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(outArray));
        }


        public void Install(RouteBuilder routeBuilder)
        {
            routeBuilder.MapGet(@"swagger/serviceSchema.json", conext =>
            {
#if DEBUG
                BuildServiceSchema();
#else
                if (swaggerDefinition == null)
                {
                    BuildServiceSchema();
                }
#endif
                conext.Response.ContentType = "text/json; charset=UTF-8";
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;

                return conext.Response.WriteAsync(JsonConvert.SerializeObject(swaggerDefinition, settings));
            });
            routeBuilder.MapGet(@"swagger/{url}", conext =>
            {
                return Swagger(conext);
            });
            routeBuilder.MapGet(@"swagger/{url}/{subdir}", conext =>
            {
                return Swagger(conext);
            });


        }
    }
    public static class OperationHelpers
    {
        public static T GetAttribute<T>(this MethodInfo methodInfo) where T : System.Attribute
        {
            var customTypes = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes<T>();
            var attribute = customTypes != null ? customTypes.SingleOrDefault(x => x.GetType() == typeof(T)) : (System.Attribute)null;
            return (T)attribute;
        }

        public static IEnumerable<string> GetPathParts(this string uri)
        {
            string pattern = "{(.*?)}";
            var result = Regex.Matches(uri, pattern);
            return result.Select(x => x.Value.Replace(@"{", string.Empty).Replace(@"}", string.Empty));
        }
        private static Dictionary<Type, string> typeDict = new Dictionary<Type, string>()
        {
            { typeof(Int16),"integer"},
            { typeof(Int32),"integer"},
            { typeof(Int64),"integer"},
            { typeof(UInt16),"integer"},
            { typeof(UInt32),"integer"},
            { typeof(UInt64),"integer"},
            { typeof(float),"number"},
            { typeof(decimal),"number"},
            { typeof(bool),"boolean"},
            { typeof(DateTime),"date"},

            { typeof(Nullable<Int16>),"integer"},
            { typeof(Nullable<Int32>),"integer"},
            { typeof(Nullable<Int64>),"integer"},
            { typeof(Nullable<UInt16>),"integer"},
            { typeof(Nullable<UInt32>),"integer"},
            { typeof(Nullable<UInt64>),"integer"},
            { typeof(Nullable<float>),"number"},
            { typeof(Nullable<decimal>),"number"},
            { typeof(Nullable<bool>),"boolean"},
            { typeof(Nullable<DateTime>),"date"},

            { typeof(string),"string"}
        };
        public static string GetJavascriptType(this Type type)
        {
            string outType = "";
            return typeDict.TryGetValue(type, out outType) ? outType : type.IsArray ? "array" : (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))) ? "array" : "object";
        }
        public static Dictionary<string, object> GetPropertyDescription(this Type type, Swagger swagger, List<Type> generatedTypes)
        {

            var objectDefinition = new objectDefiniton
            {

            };

            var propertyDescription = new Dictionary<string, object>() {
                { "type", type.GetJavascriptType() }
            };

            if (!generatedTypes.Contains(type))
            {
                generatedTypes.Add(type);
                if (type.Name == "Meal")
                {
                    Console.WriteLine("Meal");
                }

                if (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "$ref", "#/definitions/" + type.GenericTypeArguments.First().Name } });

                    objectDefinition.AddProperty(type.GenericTypeArguments.First().GetProperties().Select(x =>
                    {
                        return new property()
                        {
                            name = x.Name,
                            propertyDescription = x.PropertyType.GetPropertyDescription(swagger, generatedTypes)

                        };
                    }));

                    swagger.AddObjectDefinition(type.GenericTypeArguments.First().Name, objectDefinition);
                }

                else if (propertyDescription["type"] == "object")
                {
                    objectDefinition.AddProperty(type.GetProperties().Select(x =>
                                            {
                                                return new property()
                                                {
                                                    name = x.Name,
                                                    propertyDescription = x.PropertyType.GetPropertyDescription(swagger, generatedTypes)

                                                };
                                            }));

                    swagger.AddObjectDefinition(type.Name, objectDefinition);

                    propertyDescription.Add("$ref", "#/definitions/" + type.Name);

                }

                if (Nullable.GetUnderlyingType(type) != null)
                {
                    propertyDescription.Add("nullable", "true");
                }
            }
            return propertyDescription;
        }
    }


}
