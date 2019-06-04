using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Extensions.HealthCheck.Messages
{
    public class GetHealthCheckResponse : IResponse
    {
        public GetHealthCheckResponse()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        public string Message { get; set; }

        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}