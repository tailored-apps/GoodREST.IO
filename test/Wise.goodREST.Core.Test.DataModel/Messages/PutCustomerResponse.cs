using System;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    public class PutCustomerResponse : IResponse
    {
        public string CorrelationId        { get; set; }

        public int HttpStatusCode        { get; set; }
    }
}