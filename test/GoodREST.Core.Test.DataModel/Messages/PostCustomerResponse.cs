using System;
using GoodREST.Interfaces;

namespace GoodREST.Core.Test.DataModel.Messages
{
    public class PostCustomerResponse : IResponse
    {
        public string CorrelationId        { get; set; }
        public string asd { get; set; }
        public int HttpStatusCode        { get; set; }
    }
}