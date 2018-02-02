﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace goodREST.Interfaces
{
    public interface IRequestResponseSerializer 
    {

        string Serialize<T>(T o);
        T Deserialize<T>(string o);
        object Deserialize(Type type, string o);
        string ContentType { get;  }
    }
}