using Microsoft.AspNetCore.Routing;

namespace GoodREST.Middleware.Interface
{
    public interface IExtension
    {
        void Install(RouteBuilder routeBuilder);
    }
}