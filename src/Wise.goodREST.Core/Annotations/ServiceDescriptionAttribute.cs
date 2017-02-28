using System;

namespace Wise.goodREST.Core.Annotations
{
    public class ServiceDescriptionAttribute : Attribute
    {
        private string description;

        public ServiceDescriptionAttribute(string description)
        {
            this.description = description;
        }

        public string Description { get { return description; } }
    }
}
