using System;
using goodREST.Interfaces;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    public class PutCustomerResponse : IResponse
    {
        public string CorrelationId        { get; set; }

        public int HttpStatusCode        { get; set; }
    }
}