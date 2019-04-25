using System;
using Microsoft.AspNetCore.Hosting;
using GoodREST.Client;
using GoodREST.Core.Test.DataModel.Messages;
using Xunit;
using System.IO;
using WebApplication;

namespace Tests
{
    public class Tests
    {
        private readonly string HOST_LOCAL_TEST = @"http://localhost:4323/";

        [Fact]
        public void Test1()
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
            var resp = client.Get<GetCustomerResponse, GetCustomer>(new GetCustomer() { UserName = "asd" });

            Assert.True(resp.Customers.Count == 2);
            host.Dispose();
        }


        [Fact]
        public void Test2()
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
            var resp = client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new GoodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.asd == "asd");

            host.Dispose();
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
            var resp = client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new GoodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.asd == "asd");

            host.Dispose();
        }
    }
}
