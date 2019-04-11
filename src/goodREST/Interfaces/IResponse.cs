using System.Collections.Generic;

namespace GoodREST.Interfaces
{

    public interface IResponse
    {
        int HttpStatusCode { get; set; }
        string HttpStatus { get; set; }
        ICollection<string> Errors { get; set; }
        ICollection<string> Warnings { get; set; }
    }
}