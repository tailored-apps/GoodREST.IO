using Microsoft.AspNetCore.Http;

namespace GoodREST.Middleware.Interface
{
    public interface IRequestProvider
    {
        IHeaderDictionary GetHeader();
    }
}