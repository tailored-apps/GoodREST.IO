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
                
                externalDocs = new externalDocs { description = "Find out more about Swagger", url = "http://swagger.io" }
            };
            
            #region objectDefinitions
            var petDefinition = new objectDefiniton { type = "object", RequiredProperties = new[] { "name", "photoUrls" } };
            petDefinition.AddProperty(new property {name = "id", propertyDescription= new propertyDescription { format="int64",type="integer"} });
            swaggerDefinition.AddObjectDefinition("Pet", petDefinition);
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
            var @pathDesc = new pathDescription
            {
                tags = new[] { "pet" },
                summary = "Add a new pet to the store",
                description = string.Empty,
                operationId = "addPet",
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

            swaggerDefinition.AddOperation("/pet", "post", pathDesc);
            #endregion pathDescription
            var serializedJson = JsonConvert.SerializeObject(swaggerDefinition);
            Console.WriteLine(serializedJson);
        }

    }
}