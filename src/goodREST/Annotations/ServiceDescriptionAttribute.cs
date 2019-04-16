using System;

namespace GoodREST.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceDescriptionAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string DocDescription { get; set; }

        public string DocUrl { get; set; }
    }
}
