# Architecture
## Message-based WebServices
### Idea
Message based system architecture is actually architecture which focus on data processed throuhght system and facilitation of its communication layer in given middleware.
One of core benefits of that approach is that we have typed end-to-end API.  
### Advanages
1. Messages are well defined and are natural service communication definition
2. Enables scaling of solution in easy way, by scaling out and scaling up.
3. Enables decomposition of large monolith actions into multiple smaller requests

## DataModel
That part of solution is used for orchestration of message  objects and domain models which represents some data structures. For more  detailed information you can refer [Messages](.\DataModel\messages.md) and [Models](.\DataModel\models.md)
We highly recommend to follow [Good Practices](.\DataModel\good-practices.md) when you starting designing your first Message Based Web Service