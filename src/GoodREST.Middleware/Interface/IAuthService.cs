using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Middleware.Interface
{
    public interface IAuthService
    {
        string AuthUrl { get; }
        string AuthUser(string login, string pass, string salt);
         string PassGen(string password, string salt);
        bool CheckAccess(string xauth);
    }
}
