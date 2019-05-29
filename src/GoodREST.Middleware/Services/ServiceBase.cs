using GoodREST.Middleware.Interface;

namespace GoodREST.Middleware.Services
{
    public abstract class ServiceBase
    {
        public ISecurityService SecurityService { get; set; }
    }
}