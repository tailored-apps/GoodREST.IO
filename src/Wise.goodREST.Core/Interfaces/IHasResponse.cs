namespace Wise.goodREST.Core.Interfaces
{
    public interface IHasResponse<T>  where T : IResponse
    {
        string CorrelationId { get; set; }
    }

}