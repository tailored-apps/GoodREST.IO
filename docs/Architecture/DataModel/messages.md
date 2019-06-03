# Architecture
## Messages
In GoodREST.IO we uses two main object for defining data structure used for communication  with services:
### Request
Request objects are used for orchestration and definition how endpoint will behave and how data will be processed in service layer.
We can highlight few major features of request objects:
1. Route Definiton
2. Operation Verb 
3. Response object mapping throught inheritance from `IHaveResponse<T>` interface where T is type of returned message
4. Parameters orchestration which are used in service call.

For Example we can show simple message definiton used in our QuickStart Example

```
using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    [Route("FooBar", HttpVerb.GET)]
    public class GetBar : IHasResponse<GetBarResponse>
    {

    }
}
```
this is simplest example of request message definition only two requirements which such object should have is `Route` annotation and `IHasResponse<>` inheritance 

#### Route Attribute
Route attribute are used for marking service url path and marking verb how REST api will be used.
we can highlight supported verbs:
```
HttpVerb.GET    // used for reading opperations
HttpVerb.POST   // using for create operations
HttpVerb.PUT    // using for updating operations
HttpVerb.DELETE // using for data removal operations
HttpVerb.PATCH  // using for custom data updating operations
```
Route Path  is used for defining path of url on which message will be possible to execute through service layer
i.e. we would like to create some specyfic api on which we will be able to get All customers so our Path could look similar to this: `api\customers` that endpoint doesn't require any parameters basically by calling that endpoint we can get all customers data available in systems.
what we need to do if we would like to get specyfic cystomer? 

Easiest way is to pass CustomerId which corresponds to customer on which we are intrested and filter data by that parameter in service layer.
for example definition of message will look simillary to:

```
using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    [Route("api\customer\{CustomerId}", HttpVerb.GET)]
    public class GetCustomer: IHasResponse<GetCustomerResponse>
    {
        public int CustomerId {get; set;}
    }
}
```
as you can see we introduced new parameter `CustomerId` which should be curly braced in Path definition and we should add same parameter to class object so we can easily access parameter data from message object.

#### Adding custom parameters in message
For some operations you might need to pass more than one parameter shared throught path, purpose of that data is to have abilitiy of updating or creating new objects in system.
So for example lets suppose we would like to create new Customer object in system with given name and address , what we should actually do ? 

First step will be defining request message object with new POST verb, next step we should think what data we would like to use in that message 
for our case it will be `Name, Address, City, PostalCode, Country`  and url which will be responsible for handling that operation will be `api\customer`

let's see on following example 

```
using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    [Route("api\customer", HttpVerb.POST)]
    public class PostCustomer: IHasResponse<PostCustomerResponse>
    {
        public string Name {get; set;}
        public string Address {get; set;}
        public string City {get; set;}
        public string PostalCode {get; set;}
        public string Country {get; set;}
    }
}
```
as You probably noticed we just putted properties on message class there is nothing else just that. 
other important part is Response message definition which is PostCustomerResponse which will return Customer object of newly creatd customer and that scenario we will cover in Response part:
### Response
Response object are used for defining response message structure. One requirement for that objects is that those implements `IResponse` interface so service will be able to correctly process data throught GoodREST libriary

```
using GoodREST.Annotations;
using GoodREST.Enums;
using GoodREST.Interfaces;

namespace Contoso.Foo.WebApi.Messages
{
    public class PostCustomerResponse : IResponse
    {
        public Model.Customer Customer { get; set; }
        public int HttpStatusCode { get; set; }
        public string HttpStatus { get; set; }
        public ICollection<string> Errors { get; set; }
        public ICollection<string> Warnings { get; set; }
    }
}
```
As you cen see in example we referenced Model object fomr Model namespace called Customer, reason why we did that, is extraction of complex data structures into specyfic models which orchestrate domain of that object.
By inheriting `IRequest` interface we have to add  few properties to our response object 
```
public int HttpStatusCode { get; set; }
public string HttpStatus { get; set; }
public ICollection<string> Errors { get; set; }
public ICollection<string> Warnings { get; set; }
```
**HttpStatusCode** is used for representation of status code after exectuing application action we have following groups of codes :
```
1xx (Informational): The request was received, continuing process
2xx (Successful): The request was successfully received, understood, and accepted
3xx (Redirection): Further action needs to be taken in order to complete the request
4xx (Client Error): The request contains bad syntax or cannot be fulfilled
5xx (Server Error): The server failed to fulfill an apparently valid request
``` 
In Addtion to code we have Text representation of status under **HttpStatus** property
