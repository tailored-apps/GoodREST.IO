using Wise.goodREST.Core.Interfaces;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Middleware
{
    public interface IRestModel
    {
       void  RegisterMessageModel<T>() ;
        void RegisterSercice<T>() where T : ServiceBase;
    }
}