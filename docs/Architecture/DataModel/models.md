# Architecture
## Models
Purpose of model object is to define structure of data which represents some kind of business data. 
In Example we may want to model Customer data
One of key properties of Customer are: Name, Address, City, PostalCode, Country. So rather of copy-pasting such properties all over we can extract them into specyfic models.
i.e.

```
using System;
namespace Contoso.Foo.WebApi.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}

```
