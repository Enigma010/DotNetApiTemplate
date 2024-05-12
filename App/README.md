# App
Contains the core business logic of the application and objects that allow external components to interact with the core business logic. Placing application business logic at this level means that it can be invoked by any method: API, console application, batch job, unit test, etc.

## Guidelines
The following describes guidelines for the architecture.

### Commands
* Represent correlated data to be passed into service or entity methods.
* Used to more easily pass data rather than having a bunch of separate parameters passed in individually.

### Entities
* Entities should comprise the data as well as methods for safely changing the state of the data of the entity.
* Methods on entities should correspond to business actions that occur to the entity.  As an example for an order entity there might be a method called Cancel() that is invoked when an order is cancelled.
* All methods should adhere to [idempotence](https://en.wikipedia.org/wiki/Idempotence). While not important for the persistance of the data, this will become important when events are triggered from methods.
* Properties should allow read access to data but all data changes should be done through a method.  This allows for the safe guarding of the object state and allows multiple properties to be changes in consistant manners.
* Adhere to keeping the number of entities in a single API to a minimum.  The best practice is to have a single aggregate, or root, entity that your API deals with. Keeping the entities to a minimum will keep your projects smaller and more concise and avoid monolithic applications.

### Repositories
* Keep the access patterns data storage agnostic. This will allow an easier transition between data stores.
* Repositories should implement paging.

### Services
* Services encapsulate interacting with the repository.
* They provide access to the entities other parts of the system.

### StateChanges
* State changes represent changes that have occurred to an entity through a command.
* State changes can be emitted through the event bus to signal changes that have occurred in the system.