using GoodREST.Annotations;
using GoodREST.Core.Test.DataModel.Messages;
using GoodREST.Extensions;
using GoodREST.Middleware.Services;
using System;

namespace GoodREST.Core.Test.Services
{
    [ServiceDescription(Description = "Customer Service")]
    public class CustomerService : ServiceBase
    {
        private readonly IMockingRepository mockingRepository;

        public CustomerService(IMockingRepository mockingRepository)
        {
            this.mockingRepository = mockingRepository;
        }

        public GetCustomersResponse Get(GetCustomers request)
        {
            var response = new GetCustomersResponse();
            try
            {
                response.Customers = this.mockingRepository.GetCustomers();

                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }

        public PostCustomerResponse Post(PostCustomer request)
        {
            var response = new PostCustomerResponse();
            try
            {
                response.Customer = mockingRepository.Save(request.Customer);
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }

        public PutCustomerResponse Put(PutCustomer request)
        {
            var response = new PutCustomerResponse();
            try
            {
                var customerToUpdate = mockingRepository.GetCustomer(request.Id);
                response.Customer = mockingRepository.Update(customerToUpdate, request.Customer);
                response.Ok();
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }
            return response;
        }

        public DeleteCustomerResponse Delete(DeleteCustomer request)
        {
            var response = new DeleteCustomerResponse(); try
            {
                response.HttpStatusCode = 200;
            }
            catch (Exception ex)
            {
                response.ConvertExceptionAsError(ex, 500);
            }

            return response;
        }
    }
}