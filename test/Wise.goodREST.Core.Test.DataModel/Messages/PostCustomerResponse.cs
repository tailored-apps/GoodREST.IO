﻿using System;
using goodREST.Interfaces;

namespace Wise.goodREST.Core.Test.DataModel.Messages
{
    public class PostCustomerResponse : IResponse
    {
        public string CorrelationId        { get; set; }
        public string asd { get; set; }
        public int HttpStatusCode        { get; set; }
    }
}