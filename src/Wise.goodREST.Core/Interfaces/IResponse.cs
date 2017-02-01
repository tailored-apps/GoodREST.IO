namespace Wise.goodREST.Core.Interfaces
{

    public interface IResponse
    {
        string CorrelationId { get; set; }
        int HttpStatusCode { get; set; }
    }
}