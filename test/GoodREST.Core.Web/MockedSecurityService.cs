using GoodREST.Middleware.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodREST.Core.Web
{
    public class MockedSecurityService : ISecurityService
    {
        public string GetAuthToken()
        {
            return string.Empty;
        }

        public IEnumerable<string> GetCurrentUserRoles()
        {
            return new string[] { "TestRole" };
        }

        public string GetUserId()
        {
            return "UserId@goodrest.io";
        }

        public bool IsUserInRole(string role)
        {
            return GetCurrentUserRoles().Any(x => x == role);
        }
    }
}
