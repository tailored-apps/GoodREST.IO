using System;
using Wise.goodREST.Core.Client;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Xunit;

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void Test1() 
        {
            JsonClient client = new JsonClient("http://localhost:54601/");
            var resp = client.Get<GetCustomerResponse, GetCustomer>(new GetCustomer() {UserName= "asd" });
            Assert.True(true);
        }
    }
}
