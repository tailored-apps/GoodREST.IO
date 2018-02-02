using System;
using System.Collections.Generic;
using System.Text;

namespace goodREST.Interfaces
{
    public interface ICorrelation
    {
        string CorrelationId { get; set; }
    }
}
