# Architecture
## Good practices for DataModel objects. 
For having maximum benefits of using message-based architecture we recommend to extract DataModel into separate library project.
For maximum compatability level we are suggesting to create that project with usage of .NET Standard, so you can share that project throught .Net Framework Clients and .Net Core Clients

We highly recommend to orchestrate messages such way that we can support logical separation of messages by orchestation them in separate folders and subfolders and using corresponding namespaces for objects:

CQRS: If you plan to use Command Query Responsibility Segregation pattern we highly recommend to add aditional folders which will be used for Commands and Queries: ie:

```
Contoso.Foo.DataModel
+-- Clients
|   +-- Messages
|   |   +-- Commands
|   |   |   +-- PostClient
|   |   |   +-- PostClientResponse
|   |   +-- Queries
|   |       +-- GetClients
|   |       +-- GetClientsResponse
|   +-- Models
|       +-- Client
+-- Orders
|   +-- Messages
|   |   +-- Commands
|   |   |   +-- PostOrder
|   |   |   +-- PostOrderResponse
|   |   +-- Queries
|   |       +-- GetOrdersForClient
|   |       +-- GetOrdersForClientResponse
|   |       +-- GetProcessedOrders
|   |       +-- GetProcessedOrdersResponse
|   +-- Models
|       +-- Order
|       +-- Item
+-- Vendors
    +-- Messages
    |   +-- Commands
    |   |   +-- PostVendor
    |   |   +-- PostVendorResponse
    |   +-- Queries
    |   |   +-- GetVendorById
    |   |   +-- GetVendorByIdResponse
    +-- Models
        +-- Vendor

```