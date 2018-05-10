using System;
using System.Collections.Generic;
using System.Text;

namespace GoodREST.Interfaces
{
    public interface ICorrelation
    {
        string CorrelationId { get; set; }
    }
}
