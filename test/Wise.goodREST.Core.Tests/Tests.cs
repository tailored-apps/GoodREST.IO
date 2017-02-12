using System;
using Wise.goodREST.Core.Client;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Xunit;

namespace Tests
{
    public class Tests
    {
        private const string HOST = @"http://piwko.nieprzecietny.pl:1111";
        private const string HOST_LOCAL = @"http://localhost:54601/";
        [Fact]
        public void Test1() 
        {
            JsonClient client = new JsonClient(HOST_LOCAL);
            var resp = client.Get<GetCustomerResponse, GetCustomer>(new GetCustomer() {UserName= "asd" });
            
            Assert.True(resp.Customers.Count == 2);
        }


        [Fact]
        public void Test2()
        {
            JsonClient client = new JsonClient(HOST_LOCAL);
            var resp = client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new Wise.goodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.asd== "asd");
        }
    }
}
