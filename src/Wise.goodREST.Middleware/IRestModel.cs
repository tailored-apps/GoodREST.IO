using Wise.goodREST.Core.Interfaces;

namespace Wise.goodREST.Middleware
{
    public interface IRestModel
    {
        void RegisterMessageModel<T>() ;
    }
}