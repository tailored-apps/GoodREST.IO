using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Core.Test.DataModel.Messages
{
    public class PostCustomerResponse : IResponse
    {
        public string CorrelationId { get; set; }
        public DTO.Customer Customer { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}