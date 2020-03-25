using GoodREST.Enums;
using System;

namespace GoodREST.Annotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Authorization : Attribute
    {
    }
}