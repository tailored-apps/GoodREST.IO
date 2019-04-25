using System;
using System.Collections.Generic;
using System.Linq;
using GoodREST.Core.Test.DataModel.DTO;
using Moq;

namespace GoodREST.Core.Test.Services
{
    public class MoqRepository : IMockingRepository
    {
        private readonly Moq.Mock<IMockingRepository> mock = new Moq.Mock<IMockingRepository>();

        public MoqRepository()
        {
            mock.Setup(x => x.DeleteCustomer(It.IsAny<int>()));
            mock.Setup(x => x.GetCustomers()).Returns(new List<Customer>()
            {
                new Customer { Id = 1, Name = Guid.NewGuid().ToString(), Address = Guid.NewGuid().ToString() },
                new Customer { Id = 2, Name = Guid.NewGuid().ToString(), Address = Guid.NewGuid().ToString() }
            });

            mock.Setup(x => x.GetCustomer(It.IsAny<int>())).Returns<int>((x) => mock.Object.GetCustomers().Single(cust => cust.Id == x));

            mock.Setup(x => x.Save(It.IsAny<Customer>())).Returns<Customer>((x) => { x.Id = mock.Object.GetCustomers().Max(c => c.Id) + 1; return x; });
            mock.Setup(x=>x.Update(It.IsAny<Customer>(), It.IsAny<Customer>())).Returns<Customer, Customer>((x,y) => { 
                y.Id = x.Id;
                return y; });
            mock.Setup(x => x.DeleteCustomer(It.IsAny<int>()));
        }

        public void DeleteCustomer(int id)
        {
            mock.Object.DeleteCustomer(id);
        }

        public Customer GetCustomer(int id)
        {
            return mock.Object.GetCustomer(id);
        }

        public ICollection<Customer> GetCustomers()
        {
            return mock.Object.GetCustomers();
        }

        public Customer Save(Customer customer)
        {
         return   mock.Object.Save(customer);
        }

        public Customer Update(Customer customerToUpdate, Customer customer)
        {
            return mock.Object.Update(customerToUpdate, customer);
        }
    }
}
