using GoodREST.Core.Test.DataModel.DTO;
using System.Collections.Generic;

namespace GoodREST.Core.Test.Services
{
    public interface IMockingRepository
    {
        ICollection<Customer> GetCustomers();

        Customer GetCustomer(int id);

        Customer Save(Customer customer);

        void DeleteCustomer(int id);

        Customer Update(Customer customerToUpdate, Customer customer);
    }
}