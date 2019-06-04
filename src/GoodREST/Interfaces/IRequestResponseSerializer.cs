using System;

namespace GoodREST.Interfaces
{
    public interface IRequestResponseSerializer
    {
        string Serialize<T>(T o);

        T Deserialize<T>(string o);

        object Deserialize(Type type, string o);

        string ContentType { get; }
    }
}