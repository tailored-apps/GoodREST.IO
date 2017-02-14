using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Wise.goodREST.Middleware.Interface;
using System.Text;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Routing;

namespace Wise.goodREST.Extensions.SwaggerExtension
{
    public class SwaggerExtension : IExtension
    {
        public SwaggerExtension()
        {

        }

        private Task Swagger(HttpContext builder)
        {

            var assembly = typeof(SwaggerExtension).GetTypeInfo().Assembly;
            var requestResourceName = @"Wise.goodREST.Extensions.SwaggerExtension.swagger.dist" + builder.Request.Path.Value.Replace(@"swagger/", string.Empty).Replace("/", ".");
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
            return  Convert.ToBase64String( Encoding.UTF8.GetBytes(outArray));
        }

        public Task Install(HttpContext builder)
        {
            throw new NotImplementedException();
        }

        public void Install(RouteBuilder routeBuilder)
        {
            routeBuilder.MapGet(@"swagger/{url}", conext =>
            {
                return Swagger(conext);
            }); routeBuilder.MapGet(@"swagger/{url}/{subdir}", conext =>
            {
                return Swagger(conext);
            });

           
        }
    }
}
