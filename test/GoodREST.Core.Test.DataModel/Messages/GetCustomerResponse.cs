using GoodREST.Core.Test.DataModel.DTO;
using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Core.Test.DataModel.Messages

{
    public class GetCustomersResponse : IResponse
    {
        public ICollection<Customer> Customers { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}