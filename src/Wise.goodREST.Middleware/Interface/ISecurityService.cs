using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace GoodREST.Middleware.Interface
{
    public interface ISecurityService
    {
        IEnumerable<string> GetCurrentUserRoles();

        bool IsUserInRole(string role);

        string GetAuthToken();
    }
}
