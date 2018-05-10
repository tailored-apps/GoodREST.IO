using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GoodREST.Middleware.Interface;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Routing;
using GoodREST.Middleware;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using GoodREST.Annotations;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class SwaggerExtension : IExtension
    {
        private IRestModel model;
        private Swagger swaggerDefinition;
        private readonly IHostingEnvironment env;
        public SwaggerExtension(IRestModel restModel, IHostingEnvironment env)
        {
            this.env = env;
            model = restModel;
           
            
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
                host = System.Environment.MachineName,
                basePath = env.WebRootPath,
                schemes = new[] { "http" },

                externalDocs = new externalDocs { description = "Find out more about Swagger", url = "http://swagger.io" }
            };

            #region objectDefinitions
            foreach (var modelToRegister in model.GetRouteForType())
            {


                var petDefinition = new objectDefiniton { type = "object", RequiredProperties = new[] { "name", "photoUrls" } };
                petDefinition.AddProperty(new property { name = "id", propertyDescription = new propertyDescription { format = "int64", type = "integer" } });
                swaggerDefinition.AddObjectDefinition(modelToRegister.Value.Name, petDefinition);

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
                    tags = new[] { item.Key.Key },
                    summary = method.DeclaringType.GetTypeInfo().GetCustomAttribute<ServiceDescriptionAttribute>().Description,
                    description = method.DeclaringType.GetTypeInfo().GetCustomAttribute<ServiceDescriptionAttribute>().Description,
                    operationId = method.Name,
                    consumes = new[] { "application/json", "application/xml" },
                    produces = new[] { "application/xml", "application/json" },
                };

                pathDesc.AddSecurity(new verbSecurity { value = "petstore_auth", operations = new[] { "write:pets", "read:pets" } });
                pathDesc.AddResponse(new response { code = "405", description = new responseDescription { description = "Invalid input" } });
                pathDesc.AddParameter(new parameter
                {
                    @in = "body",
                    name = "body",
                    description = "Pet object that needs to be added to the store",
                    required = true,
                    schema = new Dictionary<string, string>() { { "$ref", "#/definitions/Pet" } }
                });

                swaggerDefinition.AddOperation(item.Key.Key, item.Key.Value.ToString(), pathDesc);

            }
            //var @pathDesc = new pathDescription
            //{
            //    tags = new[] { "pet" },
            //    summary = "Add a new pet to the store",
            //    description = string.Empty,
            //    operationId = "addPet",
            //    consumes = new[] { "application/json", "application/xml" },
            //    produces = new[] { "application/xml", "application/json" },
            //};

            //pathDesc.AddSecurity(new verbSecurity { value = "petstore_auth", operations = new[] { "write:pets", "read:pets" } });
            //pathDesc.AddResponse(new response { code = "405", description = new responseDescription { description = "Invalid input" } });
            //pathDesc.AddParameter(new parameter
            //{
            //    @in = "body",
            //    name = "body",
            //    description = "Pet object that needs to be added to the store",
            //    required = true,
            //    schema = new Dictionary<string, string>() { { "$ref", "#/definitions/Pet" } }
            //});

            //swaggerDefinition.AddOperation("/pet", "post", pathDesc);
            #endregion pathDescription
        }

        private Task Swagger(HttpContext builder)
        {

            var assembly = typeof(SwaggerExtension).GetTypeInfo().Assembly;
            var requestResourceName = @"GoodREST.Extensions.SwaggerExtension.swagger.dist" + builder.Request.Path.Value.Replace(@"swagger/", string.Empty).Replace("/", ".");
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
                return conext.Response.WriteAsync(JsonConvert.SerializeObject(swaggerDefinition));
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
}
