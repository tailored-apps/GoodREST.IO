using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wise.goodREST.Middleware.Interface
{
    public interface IRequestResponseSerializer 
    {

        string Serialize<T>(T o);
        T Deserialize<T>(string o);
        string ContentType { get;  }
    }
}
