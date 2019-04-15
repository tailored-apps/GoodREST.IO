using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodREST.Extensions.SwaggerExtension.Auxillary
{
    public static class TypeHelpers
    {
        public static IEnumerable<Type> GetNeastedTypes(this Type type)
        {
            var types = type.GetProperties().Where(x => x.PropertyType != typeof(string) && x.PropertyType.IsClass).Select(x => x.PropertyType);


            return types;
        }
    }
}
