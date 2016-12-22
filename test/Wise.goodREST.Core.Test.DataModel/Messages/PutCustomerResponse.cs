using System;
using Wise.goodREST.Core.Interfaces;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    public class PutCustomerResponse : IResponse
    {
        public string CorrelationId        { get; set; }

        public int HttpStatusCode        { get; set; }
    }
}