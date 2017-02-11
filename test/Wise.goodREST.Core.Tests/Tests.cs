using System;
using Wise.goodREST.Core.Client;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Xunit;

namespace Tests
{
    public class Tests
    {
        private const string HOST = @"http://piwko.nieprzecietny.pl:1111"; 
        [Fact]
        public void Test1() 
        {
            JsonClient client = new JsonClient(HOST);
            var resp = client.Get<GetCustomerResponse, GetCustomer>(new GetCustomer() {UserName= "asd" });
            Assert.True(true);
        }
    }
}
