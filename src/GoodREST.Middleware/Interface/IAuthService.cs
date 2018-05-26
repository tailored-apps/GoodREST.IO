using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Middleware.Interface
{
    public interface IAuthService
    {
        string AuthUrl { get; }
        bool AuthUser(string login, string pass, string salt);
        bool CheckAccess(string xauth);
    }
}
