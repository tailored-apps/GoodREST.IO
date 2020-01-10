using GoodREST.Annotations;
using GoodREST.Extensions.SwaggerExtension.Auxillary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GoodREST.Extensions.SwaggerExtension
{
    public static class OperationHelpers
    {
        public static tag GetServiceTag(this Type type)
        {
            var annotationData = type.GetCustomAttribute<ServiceDescriptionAttribute>() ?? new ServiceDescriptionAttribute { Name = type.Name };
            return new tag
            {
                name = annotationData.Name,
                description = annotationData.Description,
                externalDocs = !string.IsNullOrWhiteSpace(annotationData.DocUrl) ? new doc
                {
                    description = annotationData.DocDescription,
                    url = annotationData.DocUrl
                } : null
            };
        }

        public static pathDescription GetMessageTag(this MethodInfo type, Type requestType, string operationId, IEnumerable<string> serializers)
        {
            var annotationData = requestType.GetCustomAttribute<MessageDescriptionAttribute>() ?? new MessageDescriptionAttribute { Name = type.Name };
            var @description = new pathDescription
            {
                tags = new[] { type.DeclaringType.GetServiceTag().name },
                summary = annotationData.Summary,
                description = annotationData.Description,
                operationId = operationId,
                consumes = serializers,
                produces = serializers
            };
            description.AddResponse(new response()
            {
                code = "200",
                description = new Dictionary<string, object> {
                { "description", "success" },
                { "schema", new Dictionary<string, object>() {
                    { "$ref", "#/definitions/" + type.ReturnType.Name }
                }}
            }
            });

            var responseOpertations = type.ReturnType.GetCustomAttributes<ResponseAttribute>().Select(x =>
             {
                 return new response
                 {
                     code = x.Code,
                     description = new Dictionary<string, object>
                     {
                        {"description", x.Description },
                        //{"schema", new Dictionary<string, object>() {
                        //    { "type", "object" },
                        //    { "$ref", "#/definitions/" + x.ExceptionType }
                        //}}
                     }
                 };
             });
            if (responseOpertations != null && responseOpertations.Any())
            {
                foreach (var alternativeResponse in responseOpertations)
                {
                    description.AddResponse(alternativeResponse);
                }
            }

            return description;
        }

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


        public static Dictionary<string, object> GetPropertyDescription(this Type type)
        {

            var isObject = type.GetJavascriptType() == "object";
            var propertyDescription = new Dictionary<string, object>();
            if (!isObject)
            {
                propertyDescription.Add("type", type.GetJavascriptType());
                
            }
            if (type == typeof(byte[]))
            {
                propertyDescription.Add("items", new Dictionary<string, string> { { "type", "string" }, { "format", "byte" } });
            }
            else if (type.IsEnum || (type.GenericTypeArguments.FirstOrDefault()?.IsEnum ?? false))
            {
                if (type.GenericTypeArguments.FirstOrDefault()?.IsEnum ?? false)
                {
                    propertyDescription.TryAdd("type", "string");
                    propertyDescription.Add("enum", Enum.GetNames(type.GenericTypeArguments.FirstOrDefault()));
                }
                else
                {
                    propertyDescription.Add("enum", Enum.GetNames(type));
                }

            }
            else if (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
            {
                var propertyType = type.GenericTypeArguments.First();
                if (!propertyType.IsPrimitive && propertyType != typeof(string) && propertyType != typeof(Guid))
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "$ref", "#/definitions/" + propertyType.Name } });
                }
                else if (propertyType.IsPrimitive)
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "type", propertyType.GetJavascriptType() } });
                }
                else
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "type", "string" } });
                }
            }
            else if (isObject)
            {
                propertyDescription.Add("$ref", "#/definitions/" + type.Name);
            }
            else if (typeof(DateTime) == type || (type.GenericTypeArguments.FirstOrDefault()?.GetType() == typeof(DateTime)))
            {
                propertyDescription.Add("format", "date-time");
            }

            return propertyDescription;
        }
    }
}