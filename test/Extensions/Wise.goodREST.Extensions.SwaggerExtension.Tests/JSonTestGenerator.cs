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
                #region tags
                tags = new[] { new tag
                {
                    name="pet",
                    description="Everything about your Pets",
                    externalDocs= new doc
                        {
                            description ="Find out more",
                            url ="http://swagger.io"
                        }
                },new tag
                {
                    name="store",
                    description="Access to Petstore orders",

                },new tag
                {
                    name="user",
                    description="Operations about user",
                    externalDocs= new doc
                        {
                            description ="Find out more about our store",
                            url ="http://swagger.io"
                        }
                }


            },
                #endregion tags
                schemes = new[] { "http" },
                #region paths
                paths = new Dictionary<string, IEnumerable<IDictionary<string, pathDescription>>> { { "/pet",
                new [] { new Dictionary<string, pathDescription>{
                    { "post", new pathDescription
                        {
                            tags= new[] { "pet" },
                            summary="Add a new pet to the store",
                            description=string.Empty,
                            operationId="addPet",
                            consumes= new[] {"application/json", "application/xml" },
                            produces =new[] {"application/xml", "application/json" },
                            parameters = new[]
                            {
                                new parameter
                                {
                                    @in="body",
                                    name="body",
                                    description="Pet object that needs to be added to the store",
                                    required=true,
                                    schema = new schema {@ref= "#/definitions/Pet"}
                                }
                            },
                            responses= new[] { new response {code ="405", description= new responseDescription { description="Invalid input" } } },
                            security = new[] {new verbSecurity { value="petstore_auth", operations=new[] { "write:pets", "read:pets" } } }
                        }
                    }
                }
                }
                    }
            },
                #endregion paths
                #region securityDefinition
                securityDefinition = new[] {new securityDefinition
                {
                    value="petstore_auth",
                    definition = new securityDefinitionInfo
                    {
                        type="oauth2",
                        authorizationUrl="http://petstore.swagger.io/oauth/dialog",
                        flow="implicit",
                        scopes =new[] { new scope {
                            key= "write:pets",
                            value="modify pets in your account"
                        } }
                    }
                },
                new securityDefinition() {
                    value = "api_key",
                    definition= new securityDefinitionInfo {
                        type= "apiKey",
                        name="api_key",
                        @in="header"
                    }
                }

            },
                #endregion securityDefinition
                #region objectDefinitions
                definitions = new Dictionary<string, IDictionary<string, property>> { { "Pet", new Dictionary<string,property>
                { { "object", 
                    new property
                             {
                                 name= "id",
                                 propertyDescription= new propertyDescription
                                 {
                                     type="integer",
                                     format="int64",
                                 }
                             }
                             }
                         }
                     }
                },
                #endregion objectDefinitions
                externalDocs = new externalDocs { description = "Find out more about Swagger", url = "http://swagger.io" }
            };
            var serializedJson = JsonConvert.SerializeObject(swaggerDefinition);
            Console.WriteLine(serializedJson);
        }

    }
}