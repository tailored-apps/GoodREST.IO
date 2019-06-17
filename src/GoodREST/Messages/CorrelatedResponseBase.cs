using GoodREST.Interfaces;
using System.Collections.Generic;

namespace GoodREST.Messages
{
    public class CorrelatedResponseBase : ResponseBase, ICorrelation
    {
        public string CorrelationId { get; set; }
    }
}