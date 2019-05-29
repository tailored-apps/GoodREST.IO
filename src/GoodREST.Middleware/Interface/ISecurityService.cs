using System.Collections.Generic;

namespace GoodREST.Middleware.Interface
{
    public interface ISecurityService
    {
        IEnumerable<string> GetCurrentUserRoles();

        bool IsUserInRole(string role);

        string GetAuthToken();

        string GetUserId();
    }
}