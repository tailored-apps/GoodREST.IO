using System;
using Wise.goodREST.Core.Enums;

namespace Wise.goodREST.Core.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =true)]
    public class RouteAttribute : Attribute
    {
        private string path;
        private Enum verbs;

        public RouteAttribute(string path, HttpVerb verb)
        {
            this.path = path;
            this.verbs = verb;
        }

        public RouteAttribute(string path, Enum verbs)
        {
            this.path = path;
            this.verbs = verbs;
        }
        public string Path { get { return path; } }

        public Enum Verb { get { return verbs; } }
    }
}