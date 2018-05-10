using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Middleware.Interface
{
    public interface IAuthService
    {
        string AuthUrl { get; set; }
        bool CheckAccess(string xauth);
        string AuthUser(string login, string pass);
        string GetAuthToken();
    }
}
