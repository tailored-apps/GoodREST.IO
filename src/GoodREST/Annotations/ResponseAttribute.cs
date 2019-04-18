using System;

namespace GoodREST.Annotations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ResponseAttribute : Attribute
    {
        public string Code { get; set; }

        public string Description { get; set; }
        public Type ExceptionType { get; set; }
    }
}
