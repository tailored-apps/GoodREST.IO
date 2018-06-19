using System;
using System.Collections.Generic;

namespace GoodREST.Extensions.SwaggerExtension
{
    public class objectDefiniton
    {
        public string type { get; set; }
        public IEnumerable<string> RequiredProperties { get; set; }
        public IEnumerable<property> properties { get; set; }

        public void AddProperty(property property)
        {
            if (properties == null) { properties = new List<property>(); }

            var prop = properties as List<property>;
            prop.Add(property);
        }
    }
}