using System.Collections.Generic;
using GoodREST.Interfaces;

namespace GoodREST.Extensions.HealthCheck.Messages
{
    public class CheckResponse : IResponse
    {
        public string Message { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}