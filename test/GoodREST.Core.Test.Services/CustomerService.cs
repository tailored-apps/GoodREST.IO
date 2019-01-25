using GoodREST.Annotations;
using GoodREST.Core.Test.DataModel.DTO;
using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Extensions;
using GoodREST.Middleware.Services;
using System;
using System.Collections.Generic;

namespace GoodREST.Core.Test.Services
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
            response.HttpStatusCode = 200;
            return response;
        }
        public PostCustomerResponse Post(PostCustomer request)
        {
            var response = new PostCustomerResponse();
            response.asd = request.Customer.Name;
            response.HttpStatusCode = 200;
            return response;
        }
        public PutCustomerResponse Put(PutCustomer request)
        {
            var response = new PutCustomerResponse();
            response.HttpStatusCode = 200;
            return response;
        }
        public DeleteCustomerResponse Delete(DeleteCustomer request)
        {
            var response = new DeleteCustomerResponse();
            response.HttpStatusCode = 200;
            return response;
        }
        public DeleteCustomerResponse Delete2(DeleteCustomer request)
        {
            var response = new DeleteCustomerResponse();
            try { }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex);
            }
            return response;
        }
    }
}
