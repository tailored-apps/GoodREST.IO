using GoodREST.Enums;
using System;

namespace GoodREST.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class Role : Attribute
    {
        public Role(params string[] codes)
        {
            if (codes==null | codes.Length == 0)
            {
                throw new ArgumentException();
            }
            this.Codes = codes;
        }
        public string[] Codes { get; }
    }
}