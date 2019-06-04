using GoodREST.Annotations;
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
                externalDocs = new doc
                {
                    description = annotationData.DocDescription,
                    url = annotationData.DocUrl
                }
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
                    { "type", "object" },
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

            { typeof(Enum),"string"},
            { typeof(string),"string"}
        };

        public static string GetJavascriptType(this Type type)
        {
            string outType = "";
            return typeDict.TryGetValue(type, out outType) ? outType : type.IsArray ? "array" : type.IsEnum ? "string" : (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))) ? "array" : "object";
        }

        public static Dictionary<string, object> GetPropertyDescription(this Type type)
        {
            var objectDefinition = new objectDefiniton
            {
            };

            var propertyDescription = new Dictionary<string, object>() {
                { "type", type.GetJavascriptType() }
            };
            if (type == typeof(byte[]))
            {
                propertyDescription.Add("items", new Dictionary<string, string> { { "type", "string" }, { "format", "byte" } });
            }
            else if (type.IsEnum)
            {
                propertyDescription.Add("enum", Enum.GetNames(type));
            }
            else if (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
            {
                var propertyType = type.GenericTypeArguments.First();
                if (propertyType != typeof(string))
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "$ref", "#/definitions/" + propertyType.Name } });
                }
                else
                {
                    propertyDescription.Add("items", new Dictionary<string, string> { { "type", "string" } });
                }
            }
            else if (propertyDescription["type"] == "object")
            {
                propertyDescription.Add("$ref", "#/definitions/" + type.Name);
            }

            if (Nullable.GetUnderlyingType(type) != null)
            {
                propertyDescription.Add("nullable", "true");
            }
            return propertyDescription;
        }
    }
}