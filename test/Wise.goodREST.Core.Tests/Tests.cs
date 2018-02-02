using System;
using Microsoft.AspNetCore.Hosting;
using goodREST.Client;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Xunit;
using System.IO;
using WebApplication;

namespace Tests
{
    public class Tests
    {
        private const string HOST = @"http://piwko.nieprzecietny.pl:1111";
        private const string HOST_LOCAL = @"http://localhost:54601/";
        private readonly string HOST_LOCAL_TEST = @"http://localhost:4323/";

        [Fact]
        public void Test1()
        {
            JsonClient client = new JsonClient(HOST_LOCAL);
            var resp = client.Get<GetCustomerResponse, GetCustomer>(new GetCustomer() { UserName = "asd" });

            Assert.True(resp.Customers.Count == 2);
        }


        [Fact]
        public void Test2()
        {
            JsonClient client = new JsonClient(HOST_LOCAL);
            var resp = client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new Wise.goodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.asd == "asd");
        }
        [Fact]
        public void Test22()
        {

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls(HOST_LOCAL_TEST)
                .Build();
            host.Start();

            JsonClient client = new JsonClient(HOST_LOCAL_TEST);
            var resp = client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new Wise.goodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.asd == "asd");

            host.Dispose();
        }
    }
}
