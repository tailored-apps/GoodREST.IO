using System.Collections.Generic;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    public class GetBarResponse : IResponse
    {
        public string FooBar { get; set; }
        public int HttpStatusCode { set; get; }
        public string HttpStatus { set; get; }
        public ICollection<string> Errors { set; get; }
        public ICollection<string> Warnings { set; get; }
    }
}