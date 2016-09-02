using System;
using System.Collections.Generic;
using Wise.goodREST.Core.Interfaces;
using Wise.goodREST.Core.Test.DataModel.DTO;

namespace Wise.goodREST.Core.Test.DataModel

{
    public class GetCustomerResponse : IResponse
    {
        public IList<Customer> Customers { get; set; }

        public int HttpStatusCode { get; set; }
    }
}