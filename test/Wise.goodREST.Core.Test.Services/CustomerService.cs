using System;
using System.Collections.Generic;
using Wise.goodREST.Core.Test.DataModel;
using Wise.goodREST.Core.Test.DataModel.DTO;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Core.Test.Services
{
    public class CustomerService: ServiceBase
    {
        public GetCustomerResponse Get(GetCustomer request)
        {
            var response = new GetCustomerResponse();
            response.Customers = new List<Customer>()
            {
                new Customer() { Id=1},
                new Customer() { Id=2},
            };
            return response;
        }
    }
}
