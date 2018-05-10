using System;
using System.Collections.Generic;
using System.Text;

namespace Wise.goodREST.Middleware.Interface
{
    public interface IAuthService
    {
        string GetAuthToken();
        IEnumerable<string> GetRoles();
    }
}
