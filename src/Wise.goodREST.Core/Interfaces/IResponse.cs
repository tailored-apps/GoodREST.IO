namespace Wise.goodREST.Core.Interfaces
{
    public interface IResponse<T> where T:IResponse
    {
         string CorrelationId { get; set; }
    }


    public interface IResponse
    {
        string CorrelationId { get; set; }
        int HttpStatusCode { get; set; }
    }
}