using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Messages
{
    public class ResponseBase : IResponse
    {
        public ResponseBase()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}