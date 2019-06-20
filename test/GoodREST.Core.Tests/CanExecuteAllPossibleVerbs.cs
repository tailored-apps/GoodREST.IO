using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Core.Tests.Commons;
using Xunit;

namespace Tests
{
    public class CanExecuteAllPossibleVerbs : IClassFixture<CoreServiceTestsFixture>
    {
        private CoreServiceTestsFixture fixture;

        public CanExecuteAllPossibleVerbs(CoreServiceTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void CanGet()
        {
            var resp = fixture.Client.Get<GetCustomersResponse, GetCustomers>(new GetCustomers() { });

            Assert.True(resp.Customers.Count == 2);
        }

        [Fact]
        public void CanPost()
        {
            var resp = fixture.Client.Post<PostCustomerResponse, PostCustomer>(new PostCustomer() { Customer = new GoodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.Customer.Id > 2);
        }

        [Fact]
        public void CanPut()
        {
            var resp = fixture.Client.Put<PutCustomerResponse, PutCustomer>(new PutCustomer() { Id = 1, Customer = new GoodREST.Core.Test.DataModel.DTO.Customer() { Name = "asd" } });
            Assert.True(resp.Customer.Id == 1);
            Assert.Equal("asd", resp.Customer.Name);
        }

        [Fact]
        public void CanDelete()
        {
            var resp = fixture.Client.Delete<DeleteCustomerResponse, DeleteCustomer>(new DeleteCustomer { Id = 1 });
            Assert.True(resp.HttpStatusCode == 200);
        }
    }
}