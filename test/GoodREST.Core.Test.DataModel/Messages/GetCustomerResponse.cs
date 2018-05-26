using System;
using System.Collections.Generic;
using GoodREST.Interfaces;
using GoodREST.Core.Test.DataModel.DTO;

namespace GoodREST.Core.Test.DataModel.Messages

{
    public class GetCustomerResponse : IResponse
    {
        public string CorrelationId { get; set; }

        public IList<Customer> Customers { get; set; }

        public int HttpStatusCode { get; set; }
    }
}