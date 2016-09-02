using System;
using Wise.goodREST.Core.Enums;

namespace Wise.goodREST.Core.Annotations
{
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

    }
}