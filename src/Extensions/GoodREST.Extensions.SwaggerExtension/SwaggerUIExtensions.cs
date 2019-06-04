using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoodREST.Extensions.SwaggerExtension
{
    public static class SwaggerUIExtensions
    {
        public static Task SwaggerSchema(this HttpContext builder, Swagger swagger)
        {
            builder.Response.ContentType = "text/json; charset=UTF-8";
            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            return builder.Response.WriteAsync(JsonConvert.SerializeObject(swagger, settings));
        }

        public static Task SwaggerUI(this HttpContext builder)
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

        private static string ConvertToBase64(Stream stream)
        {
            Byte[] inArray = new Byte[(int)stream.Length];
            Char[] outArray = new Char[(int)(stream.Length * 1.34)];
            stream.Read(inArray, 0, (int)stream.Length);
            Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(outArray));
        }
    }
}