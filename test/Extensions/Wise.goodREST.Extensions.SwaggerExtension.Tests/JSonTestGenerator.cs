using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Wise.goodREST.Extensions.SwaggerExtension.Tests
{
    public class JSonTestGenerator
    {
        [Fact]
        public void Test()
        {
            Swagger swaggerDefinition = new Swagger
            {
                swagger = "2.0",
                #region info
                info = new info
                {
                    description = "This is a sample server Petstore server.  You can find out more about Swagger at [http://swagger.io](http://swagger.io) or on [irc.freenode.net, #swagger](http://swagger.io/irc/).  For this sample, you can use the api key `special-key` to test the authorization filters.",
                    version = "1.0.0",
                    title = "Swagger Petstore",
                    termsOfService = "http://swagger.io/terms/",
                    contact = new contact { email = "apiteam@swagger.io" },
                    license = new license { name = "Apache 2.0", url = "http://www.apache.org/licenses/LICENSE-2.0.html" }
                },
                #endregion info
                host = "petstore.swagger.io",
                basePath = "/v2",
                schemes = new[] { "http" },




                #region securityDefinition
                securityDefinition = new Dictionary<string, securityDefinitionInfo> {
                {
                    "petstore_auth",
                     new securityDefinitionInfo
                    {
                        type="oauth2",
                        authorizationUrl="http://petstore.swagger.io/oauth/dialog",
                        flow="implicit",
                        scopes =new Dictionary<string,string> { {
                            "write:pets",
                            "modify pets in your account"
                        } }
                    }
                    },
                 {
                     "api_key",
                   new securityDefinitionInfo {
                        type= "apiKey",
                        name="api_key",
                        @in="header"
                    }
                }

            },
                #endregion securityDefinition
                #region objectDefinitions
                definitions = new Dictionary<string, IDictionary<string, object>> { { "Pet", new Dictionary<string,object>
                { {"type", "object" },
                    {  "required" , new[] { "name", "photoUrls" } },
                    { "properties", new Dictionary<string,object>
                             { {
                                 "id",
                                  new Dictionary<string,string>
                                 {
                                      { "type","integer" },
                                      { "format","int64" },
                                 }
                        }
                             }
                             }
                         }
                     }
                },
                #endregion objectDefinitions
                externalDocs = new externalDocs { description = "Find out more about Swagger", url = "http://swagger.io" }
            };

            swaggerDefinition.AddTag(new tag { name = "pet", description = "Everything about your Pets", externalDocs = new doc { description = "Find out more", url = "http://swagger.io" } });
            swaggerDefinition.AddTag(new tag { name = "store", description = "Access to Petstore orders" });
            swaggerDefinition.AddTag(new tag { name = "user", description = "Operations about user", externalDocs = new doc { description = "Find out more about our store", url = "http://swagger.io" } });

            swaggerDefinition.AddOperation("/pet", "post", new pathDescription
            {
                tags = new[] { "pet" },
                summary = "Add a new pet to the store",
                description = string.Empty,
                operationId = "addPet",
                consumes = new[] { "application/json", "application/xml" },
                produces = new[] { "application/xml", "application/json" },
                parameters = new[]
                                {
                                    new parameter
                                    {
                                        @in="body",
                                        name="body",
                                        description="Pet object that needs to be added to the store",
                                        required=true,
                                        schema = new Dictionary<string,string>() { { "$ref", "#/definitions/Pet" } }
                                    }
                                },
                                    responses = new Dictionary<string, IDictionary<string, string>> {
                                    { "405", new Dictionary<string,string>() { { "description", "Invalid input" }  } } },
                security =
                                new List<IDictionary<string, IEnumerable<string>>>
                                {
                                    {
                                        new Dictionary<string,IEnumerable<string>> { { "petstore_auth", new[]{ "write:pets", "read:pets" } } }

                                    }
                                }



            });

            var serializedJson = JsonConvert.SerializeObject(swaggerDefinition);
            Console.WriteLine(serializedJson);
        }

    }
}