using System;
using System.Collections.Generic;
using goodREST.Annotations;
using Wise.goodREST.Core.Test.DataModel;
using Wise.goodREST.Core.Test.DataModel.DTO;
using Wise.goodREST.Core.Test.DataModel.Messages;
using Wise.goodREST.Middleware.Services;

namespace Wise.goodREST.Core.Test.Services
{
    [ServiceDescription("ASD")]
    public class CustomerService : ServiceBase
    {
        public CustomerService()
        {
            Console.WriteLine(DateTime.Now.ToShortDateString());
        }
        public GetCustomerResponse Get(GetCustomer request)
        {
            var response = new GetCustomerResponse();
            response.Customers = new List<Customer>()
            {
                new Customer() { Id=1, Address="Adres numer jeden",CreateDate =new DateTime(2012,12,2),Name="user name to: "+request.UserName ,Status=23},
                new Customer() { Id=2, Address="Adres wesoły",CreateDate =new DateTime(2012,06,2),Name="Kierunek wałbrzych" ,Status=23},
            };
            return response;
        }
        public PostCustomerResponse Post(PostCustomer request)
        {
            var response = new PostCustomerResponse();
            response.asd = request.Customer.Name;
            return response;
        }
        public PutCustomerResponse Put(PutCustomer request)
        {
            var response = new PutCustomerResponse();
            return response;
        }
        public DeleteCustomerResponse Delete(DeleteCustomer request)
        {
            var response = new DeleteCustomerResponse();
            return response;
        }
    }
}
