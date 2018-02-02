﻿using System;
using System.Collections.Generic;
using goodREST.Interfaces;
using Wise.goodREST.Core.Test.DataModel.DTO;

namespace Wise.goodREST.Core.Test.DataModel.Messages

{
    public class GetCustomerResponse : IResponse
    {
        public string CorrelationId { get; set; }

        public IList<Customer> Customers { get; set; }

        public int HttpStatusCode { get; set; }
    }
}