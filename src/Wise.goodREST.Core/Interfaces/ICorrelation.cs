using System;
using System.Collections.Generic;
using System.Text;

namespace Wise.goodREST.Core.Interfaces
{
    public interface ICorrelation
    {
        string CorrelationId { get; set; }
    }
}
