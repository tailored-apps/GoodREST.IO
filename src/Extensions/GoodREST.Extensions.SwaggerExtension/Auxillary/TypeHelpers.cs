using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodREST.Extensions.SwaggerExtension.Auxillary
{
    public static class TypeHelpers
    {
        public static IEnumerable<Type> GetTypeTree(this Type type, List<Type> ListOfTypes = null)
        {
            if (ListOfTypes == null)
            {
                ListOfTypes = new List<Type>();
            }

            ListOfTypes.AddNotExistingRange(type.GetAllTypesUsedInType());
            ListOfTypes.AddIfNotExists(type);

            for (int i = 0; i < ListOfTypes.Count(); i++)
            {
                var listOFReferencedTypes = ListOfTypes[i].GetAllTypesUsedInType();
                ListOfTypes.AddNotExistingRange(listOFReferencedTypes);

                var notExisting = listOFReferencedTypes.Where(x => !ListOfTypes.Contains(x));
                ListOfTypes.AddNotExistingRange(notExisting);
                ListOfTypes.AddNotExistingRange(notExisting.SelectMany(x => x.GetTypeTree(ListOfTypes)));
            }

            return ListOfTypes;
        }

        public static IEnumerable<Type> GetAllTypesUsedInType(this Type type)
        {
            var types = type.GetProperties().Where(x => x.PropertyType != typeof(string) && (x.PropertyType.IsClass || x.PropertyType.IsEnum)).Select(x => x.PropertyType).ToList();

            var genericTypes = type.IsGenericType ? type.GenericTypeArguments : Enumerable.Empty<Type>();
            types.AddNotExistingRange(genericTypes);

            var inherit = type.GetInterfaces().Where(x => x.IsGenericType).SelectMany(x => x.GetGenericArguments());
            types.AddNotExistingRange(inherit);

            var genericProps = type.GetProperties().Where(x => x.PropertyType.IsGenericType).SelectMany(x => x.PropertyType.GetGenericArguments()).Where(x => x.IsClass && x.UnderlyingSystemType != typeof(string));

            types.AddNotExistingRange(genericProps);

            return types;
        }

        public static List<Type> AddNotExistingRange(this List<Type> type, IEnumerable<Type> enumerable)
        {
            if (enumerable != null && enumerable.Any())
            {
                foreach (var item in enumerable)
                {
                    type.AddIfNotExists(item);
                }
            }
            return type;
        }

        public static List<Type> AddIfNotExists(this List<Type> type, Type item)
        {
            if (!type.Contains(item))
            {
                type.Add(item);
            }
            return type;
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
            { typeof(DateTime),"string"},
            { typeof(Guid),"string"},

            { typeof(Nullable<Int16>),"integer"},
            { typeof(Nullable<Int32>),"integer"},
            { typeof(Nullable<Int64>),"integer"},
            { typeof(Nullable<UInt16>),"integer"},
            { typeof(Nullable<UInt32>),"integer"},
            { typeof(Nullable<UInt64>),"integer"},
            { typeof(Nullable<float>),"number"},
            { typeof(Nullable<decimal>),"number"},
            { typeof(Nullable<bool>),"boolean"},
            { typeof(Nullable<DateTime>),"string"},
            { typeof(Nullable<Guid>),"string"},

            { typeof(Enum),"string"},
            { typeof(string),"string"}
        };

        public static string GetJavascriptType(this Type type)
        {
            return typeDict.TryGetValue(type, out string outType) ? outType : type.IsArray ? "array" : type.IsEnum ? "string" : (type != typeof(string) && type.GetInterfaces().Any(i => i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(ICollection<>) || i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))) ? "array" : "object";
        }
    }
}